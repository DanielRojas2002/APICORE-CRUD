using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;

using APICORE.Models;
using System.Collections.Generic;

namespace APICORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {

        private readonly string _cadenasql;

        public ProductoController(IConfiguration config)
        {
            _cadenasql = config.GetConnectionString("bdContext");

        }


        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
             List<Producto> lista =  new List<Producto>();


            try
            {
                using (var conn = new SqlConnection(_cadenasql))
                {

                    conn.Open();
                    var cmd = new SqlCommand("sp_lista_productos", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Producto(){

                                IdProducto =Convert.ToInt32( reader["IdProducto"] ),
                                Nombre = Convert.ToString(reader["Nombre"]),
                                Categoria = Convert.ToString(reader["Categoria"]),
                                Marca = Convert.ToString(reader["Marca"]),
                                Precio = Convert.ToInt32(reader["Precio"]),
                                CodigoBarra= Convert.ToString(reader["CodigoBarra"])

                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK,new {mensaje="0k", lista = lista });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, lista = lista });
            }
        }

        [HttpGet]
        [Route ("Obtener/{IdProducto:int}")]
        public IActionResult Obtener(int IdProducto)
        {
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();

            try
            {
                using (var conn = new SqlConnection(_cadenasql))
                {

                    conn.Open();
                    var cmd = new SqlCommand("sp_lista_productos", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Producto()
                            {

                                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                Nombre = Convert.ToString(reader["Nombre"]),
                                Categoria = Convert.ToString(reader["Categoria"]),
                                Marca = Convert.ToString(reader["Marca"]),
                                Precio = Convert.ToInt32(reader["Precio"]),
                                CodigoBarra = Convert.ToString(reader["CodigoBarra"])

                            });
                        }
                    }
                }

                producto = lista.Where(i => i.IdProducto == IdProducto).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "0k", lista = producto });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message, lista = producto });
            }
        }


        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
          

            try
            {
                using (var conn = new SqlConnection(_cadenasql))
                {

                    conn.Open();
                    var cmd = new SqlCommand("sp_guardar_producto", conn);
                    cmd.Parameters.AddWithValue("codigobarra", objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                  
                }

               
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "0k" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }



        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {


            try
            {
                using (var conn = new SqlConnection(_cadenasql))
                {

                    conn.Open();
                    var cmd = new SqlCommand("sp_editar_producto", conn);
                    cmd.Parameters.AddWithValue("idProducto", objeto.IdProducto == 0 ? DBNull.Value:objeto.IdProducto);
                    cmd.Parameters.AddWithValue("codigobarra", objeto.CodigoBarra is null ? DBNull.Value: objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.Nombre is null ? DBNull.Value : objeto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.Marca is null ? DBNull.Value : objeto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.Categoria is null ? DBNull.Value : objeto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.Precio == 0 ? DBNull.Value : objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {


            try
            {
                using (var conn = new SqlConnection(_cadenasql))
                {

                    conn.Open();
                    var cmd = new SqlCommand("sp_eliminar_producto", conn);
                    cmd.Parameters.AddWithValue("idProducto", idProducto );
                
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();

                }


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }


    }
}
