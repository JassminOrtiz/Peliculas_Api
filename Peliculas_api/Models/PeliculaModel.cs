namespace Peliculas_api.Models
{
    public class PeliculaModel
    {
        public Guid ID_Pelicula { get; set; }
        public string Titulo { get; set; }
        public string Reparto { get; set; }
        public int Anio { get; set; }
        public string Genero { get; set; }
        public Guid ID_Genero { get; set; }
    }
}
