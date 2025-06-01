using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BaloncestoAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

namespace BaloncestoAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config; // <-- Campo añadido


        public AuthController(IConfiguration config)
        {
            _config = config; // Inicializar el campo
            _connectionString = config.GetConnectionString("DefaultConnection");
        }


        [HttpPost("registro")]
        public IActionResult Registrar([FromBody] RegistroRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.nombre) || string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.contraseña))
            {
                return BadRequest(new { mensaje = "Datos inválidos: nombre, email y contraseña son obligatorios." });
            }

            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();

                // ✅ Comprobamos si ya existe un usuario con ese nombre o correo
                var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM Jugadores WHERE Email = @Email OR Nombre = @Nombre", conexion);
                checkCmd.Parameters.AddWithValue("@Email", request.email);
                checkCmd.Parameters.AddWithValue("@Nombre", request.nombre);

                int existe = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (existe > 0)
                {
                    return Conflict(new { mensaje = "Ya existe un usuario con ese nombre o correo electrónico." });
                }

                // Si no existe, lo insertamos
                var comando = new MySqlCommand(
                    "INSERT INTO Jugadores (Nombre, Email, ContraseñaHash, FechaRegistro, Rol) " +
                    "VALUES (@nombre, @email, SHA2(@contraseña, 256), NOW(), @rol)",
                    conexion
                );

                comando.Parameters.AddWithValue("@nombre", request.nombre);
                comando.Parameters.AddWithValue("@email", request.email);
                comando.Parameters.AddWithValue("@contraseña", request.contraseña);
                comando.Parameters.AddWithValue("@rol", "jugador");

                comando.ExecuteNonQuery();
            }

            return Ok(new { mensaje = "Jugador registrado con éxito" });
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                var comando = new MySqlCommand(
                "SELECT Id, Rol, Nombre FROM Jugadores WHERE Email = @email AND ContraseñaHash = SHA2(@contraseña, 256)",
                conexion
                );

                comando.Parameters.AddWithValue("@email", request.email);
                comando.Parameters.AddWithValue("@contraseña", request.contraseña);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var token = GenerarTokenJWT(reader["id"].ToString(), reader["rol"].ToString());
                        var nombre = reader["nombre"].ToString();
                        var rol = reader["rol"].ToString();
                        var email = request.email; 



                        return Ok(new { token, nombre = reader["nombre"].ToString(), rol, email });

                    }

                }
            }

            return Unauthorized("Credenciales incorrectas");
        }

        // EDITAR USUARIO
        [HttpPut("editar")]
        public IActionResult EditarUsuario([FromBody] EditarUsuarioRequest request)
        {
            if (string.IsNullOrEmpty(request.email))
                return BadRequest(new { mensaje = "Email obligatorio." });

            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();

                var comandos = new List<string>();
                if (!string.IsNullOrEmpty(request.nuevoNombre))
                    comandos.Add("nombre = @nuevoNombre");
                if (!string.IsNullOrEmpty(request.nuevaContraseña))
                    comandos.Add("ContraseñaHash = SHA2(@nuevaContraseña, 256)");

                if (comandos.Count == 0)
                    return BadRequest(new { mensaje = "No se han especificado cambios." });

                var query = $"UPDATE Jugadores SET {string.Join(", ", comandos)} WHERE email = @email";
                var comando = new MySqlCommand(query, conexion);

                comando.Parameters.AddWithValue("@email", request.email);
                if (request.nuevoNombre != null)
                    comando.Parameters.AddWithValue("@nuevoNombre", request.nuevoNombre);
                if (request.nuevaContraseña != null)
                    comando.Parameters.AddWithValue("@nuevaContraseña", request.nuevaContraseña);

                int filasAfectadas = comando.ExecuteNonQuery();
                if (filasAfectadas == 0)
                    return NotFound(new { mensaje = "Usuario no encontrado." });

                return Ok(new { mensaje = "Datos actualizados correctamente." });
            }
        }

        //ELIMINAR USUARIO
        [HttpDelete("eliminar")]
        [Authorize(Roles = "admin")]
        public IActionResult EliminarUsuario([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email obligatorio.");

            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                var comando = new MySqlCommand("DELETE FROM Jugadores WHERE email = @Email", conexion);
                comando.Parameters.AddWithValue("@Email", email);

                int filasAfectadas = comando.ExecuteNonQuery();
                if (filasAfectadas == 0)
                    return NotFound("Usuario no encontrado.");

                return Ok("Usuario eliminado correctamente.");
            }
        }

        // VER USUARIOS
        [HttpGet("usuarios")]
        [Authorize(Roles = "admin")]
        public IActionResult ObtenerUsuarios()
        {
            var usuarios = new List<object>();

            using (var conexion = new MySqlConnection(_connectionString))
            {
                conexion.Open();
                var comando = new MySqlCommand("SELECT Id, Nombre, Email, Rol, FechaRegistro FROM Jugadores", conexion);

                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new
                        {
                            Id = reader["Id"],
                            Nombre = reader["Nombre"],
                            Email = reader["Email"],
                            Rol = reader["Rol"],
                            FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]).ToString("yyyy-MM-dd HH:mm")
                        });
                    }
                }
            }

            return Ok(usuarios);
        }




        private string GenerarTokenJWT(string userId, string rol)
        {
            var claveSecreta = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"])
            );

            var creds = new SigningCredentials(claveSecreta, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Role, rol)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


