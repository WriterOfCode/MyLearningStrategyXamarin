using System;
using System.Collections.Generic;
using System.Text;

namespace MyLearningStrategyMobleXForms.Models
{
    public class PickListDTO
    {
        internal int id;

        public int Id { get; set; }
        public String Definition { get; set; }
        public string Name { get; set; }
        public override string ToString() => Definition;
    }
}
