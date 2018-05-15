using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EmotionPlatzi.Models
{
    public class EmotionPlatziContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public EmotionPlatziContext() : base("name=EmotionPlatzi")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EmotionPlatziContext>());
        }

        public System.Data.Entity.DbSet<EmotionPlatzi.Models.EmoPicture> EmoPictures { get; set; }

        public System.Data.Entity.DbSet<EmotionPlatzi.Models.EmoFace> EmoFaces { get; set; }

        public System.Data.Entity.DbSet<EmotionPlatzi.Models.EmoEmotion> EmoEmotions { get; set; }

        public System.Data.Entity.DbSet<EmotionPlatzi.Models.Home> Homes { get; set; }
    }
}
