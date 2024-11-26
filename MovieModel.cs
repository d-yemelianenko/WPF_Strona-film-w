using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strona_filmów
{
    internal class MovieModel
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string Rok { get; set; }
        public string Autor { get; set; }
        public string PhotoName { get; set; }
    }
}
