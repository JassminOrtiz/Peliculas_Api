using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Peliculas_api.Models;
using System.Data;
using System.Data.SqlClient;

namespace Peliculas_api.Controllers
{
    [Route("api/[controller]")]
    public class PeliculasController : Controller
    {
        IConfiguration _context;
        public PeliculasController(IConfiguration context)
        {
            _context = context;
        }

        [HttpGet("Save_all")]
        public List<PeliculaModel> Get_All()
        {
            List<PeliculaModel> pelis = new List<PeliculaModel>();
            string connection = _context["ConnectionStrings:DefaultConnection"];

            using (SqlConnection conn = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand("List_Movies", conn))
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    conn.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            PeliculaModel pe = new PeliculaModel();

                            pe.ID_Pelicula = sdr["ID_Pelicula"] == DBNull.Value ? Guid.Empty : Guid.Parse(sdr["ID_Pelicula"].ToString());
                            pe.Titulo = sdr["Titulo"] == DBNull.Value ? "" : sdr["Titulo"].ToString();
                            pe.Reparto = sdr["Reparto"] == DBNull.Value ? "" : sdr["Reparto"].ToString();
                            pe.Anio = sdr["Anio"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["Anio"].ToString());
                            pe.Genero = sdr["Genero"] == DBNull.Value ? "" : sdr["Genero"].ToString();
                            pe.ID_Genero = sdr["ID_Genero"] == DBNull.Value ? Guid.Empty : Guid.Parse(sdr["ID_Genero"].ToString());

                            pelis.Add(pe);
                        }
                    }
                }
            }

            return pelis;
        }

        [HttpGet("Save_genero")]
        public List<GeneroModel> Get_Generos()
        {
            List<GeneroModel> gen = new List<GeneroModel>();
            string connection = _context["ConnectionStrings:DefaultConnection"];

            using (SqlConnection conn = new SqlConnection(connection))
            {
                using (SqlCommand cmd = new SqlCommand("List_Generos", conn))
                {

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    conn.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            GeneroModel pe = new GeneroModel();

                            pe.ID_Genero = sdr["ID_Genero"] == DBNull.Value ? Guid.Empty : Guid.Parse(sdr["ID_Genero"].ToString());
                            pe.Genero = sdr["Genero"] == DBNull.Value ? "" : sdr["Genero"].ToString();

                            gen.Add(pe);
                        }
                    }
                }
            }

            return gen;
        }


        [HttpPost("UpdateMovie")]
        public PeliculaModel UpdateMovie([FromBody]PeliculaModel peli)
        {
            string connectionString = _context["ConnectionStrings:DefaultConnection"];

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Update_movies", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                     
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    conn.Open();

                    // Asegúrate de cerrar la conexión después de utilizarla
                    if (peli.ID_Pelicula != Guid.Empty)
                    {
                        cmd.Parameters.AddWithValue("@ID_Pelicula", peli.ID_Pelicula);
                    }

                    cmd.Parameters.AddWithValue("@ID_Genero", peli.ID_Genero);
                    cmd.Parameters.AddWithValue("@Titulo", peli.Titulo);
                    cmd.Parameters.AddWithValue("@Reparto", peli.Reparto);
                    cmd.Parameters.AddWithValue("@Anio", peli.Anio);

                    // Ejecuta el comando sin SqlDataReader, ya que es un procedimiento almacenado de actualización
                    cmd.ExecuteNonQuery();
                }
            }

            return peli;
        }

    }
}
