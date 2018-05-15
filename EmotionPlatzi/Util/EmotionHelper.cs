using EmotionPlatzi.Models;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Reflection;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace EmotionPlatzi.Util
{
    public class EmotionHelper
    {
        public FaceServiceClient emoClient;

        public EmotionHelper(string key)
        {
            emoClient = new FaceServiceClient(key, "https://southcentralus.api.cognitive.microsoft.com/face/v1.0");
        }

        public async Task<EmoPicture> DetectAndExtracFacesAsync(Stream imageStream )
        {
            try
            {
                IEnumerable<FaceAttributeType> faceAttributes = new FaceAttributeType[] { FaceAttributeType.Emotion };
                Face[] faces = await emoClient.DetectAsync(imageStream, false, false, faceAttributes);

                var emoPicture = new EmoPicture();
                emoPicture.Faces = ExtractFaces(faces, emoPicture);
                return emoPicture;
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }

        private ObservableCollection<EmoFace> ExtractFaces(Face[] faces,EmoPicture emoPicture)
        {
            var listaFaces = new ObservableCollection<EmoFace>();
            foreach ( var face in faces)
            {
                var emoface=new EmoFace()
                {
                    X = face.FaceRectangle.Left,
                    Y = face.FaceRectangle.Top,
                    Width = face.FaceRectangle.Width,
                    Heigth = face.FaceRectangle.Height,
                    Picture = emoPicture
                    
                };
                
                emoface.Emotions = ProcessEmotions(face.FaceAttributes.Emotion,emoface);
                listaFaces.Add(emoface);
            }
            return listaFaces;
        }

        private ObservableCollection<EmoEmotion> ProcessEmotions(EmotionScores scores,EmoFace emoFace)
        {
            var emotionList = new ObservableCollection<EmoEmotion>();
            var properties= scores.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var filterProperties = properties.Where(p => p.PropertyType == typeof(float));
            var emotype = EmoEmotionEnum.Undetermined;
            foreach (var prop in filterProperties)
            {
                if(!Enum.TryParse<EmoEmotionEnum>(prop.Name, out emotype))
                    emotype = EmoEmotionEnum.Undetermined;
                var emoEmotion = new EmoEmotion();
                emoEmotion.Score = (float)prop.GetValue(scores);
                emoEmotion.EmotionType = emotype;
                emoEmotion.Face = emoFace;
                emotionList.Add(emoEmotion);
            }
            return emotionList;
        }
    }
}