using System.Linq;

namespace RouteToCodePost.Data
{
    public class SeedData
    {
        public static void Initialize(SampleContext context)
        {
            if (!context.Bands.Any())
            {
                context.Bands.AddRange(
                    new Band(1, "Led Zeppelin"),
                    new Band(2, "Arcade Fire"),
                    new Band(3, "The Who"),
                    new Band(4, "The Eagles, man")
                );

                context.SaveChanges();
            }
        }
    }
}
