using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestWebAppCoolName.Models;

namespace TestWebAppCoolName.Helpers
{
    public static class TagParser
    {
        public static List<Tag> ParseTags(string tagy, ApplicationDbContext context)
        {
            var listTags = new List<Tag>();
            if (string.IsNullOrEmpty(tagy))
            {
                return listTags;
            }
            var tags = tagy.Split('&');
            foreach (var tag in tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    var existingTag = context.Tags.FirstOrDefault(t => t.Name == tag.Trim());
                    if (existingTag != null)
                    {
                        listTags.Add(existingTag);
                    }
                }
            }

            return listTags;
        }
    }
}