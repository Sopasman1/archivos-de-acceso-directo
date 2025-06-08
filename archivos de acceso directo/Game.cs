using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace archivos_de_acceso_directo
{
    internal class Game
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Fixed size for string fields
        public const int BrandLength = 30;
        public const int NameLength = 50;

        public override string ToString()
        {
            return $"{Id} - {Brand.Trim()} - {Name.Trim()} (${Price})";
        }
    }
}
