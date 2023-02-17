using System.ComponentModel.DataAnnotations;

namespace MLNET_NET_Core.Models
{
    public class ModelLabel
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public string Name { get; set; }

        public float Score { get; set; }

        public static List<ModelLabel> GetLabels()
        {
            return new List<ModelLabel>()
                {
                    new ModelLabel { Id = 0, Label = "punching_hole", Name = "punching_hole" },
                    new ModelLabel { Id = 1, Label = "rolled_pit", Name = "rolled_pit" },
                };
        }
    }
}