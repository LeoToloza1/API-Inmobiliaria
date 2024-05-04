using inmobiliaria.Models;
using inmobiliaria.Servicio;
using Microsoft.EntityFrameworkCore;
using Repositorios;


public class RepositorioUsuario : IRepositorio<Usuario>
{
    private readonly DataContext _contexto;
    public RepositorioUsuario(DataContext contexto)
    {
        _contexto = contexto;
    }
    public bool Actualizar(Usuario usuario)
    {
        try
        {
            _contexto.Entry(usuario).State = EntityState.Modified;
            _contexto.Usuario.Update(usuario);
            _contexto.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al actualizar el usuario: {ex.Message}");
            return false;
        }
    }
    public Usuario BuscarPorId(int id)
    {
        return _contexto.Usuario.FirstOrDefault(i => i.id == id);
    }
    public bool Crear(Usuario usuario)
    {
        try
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.password))
            {
                Console.WriteLine("El usuario o la contraseña son nulos.");
                return false;
            }

            usuario.password = HashPass.HashearPass(usuario.password);
            _contexto.Usuario.Add(usuario);
            _contexto.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear el Usuario: {ex.Message}");
            return false;
        }
    }
    public bool EliminadoLogico(int id)
    {
        try
        {
            var usuario = _contexto.Usuario.FirstOrDefault(i => i.id == id);
            if (usuario != null)
            {
                usuario.borrado = true;
                _contexto.SaveChanges();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al eliminar el Usuario: {ex.Message}");
            return false;
        }
    }
    public List<Usuario> ObtenerActivos()
    {
        return _contexto.Usuario.Where(u => !u.borrado).ToList();
    }
    public List<Usuario> ObtenerTodos()
    {
        return _contexto.Usuario.ToList();
    }
    public Usuario ObtenerPorEmail(string email)
    {
        try
        {
            var usuario = _contexto.Usuario.Single(u => u.email == email);
            return usuario;
        }
        catch (InvalidOperationException)
        {
            throw new Exception("No se encontró ningún Usuario registrado con el correo electrónico especificado.");
        }
    }
    public bool CambiarPass(int usuarioID, string passwordNueva)
    {
        try
        {
            var usuario = _contexto.Usuario.FirstOrDefault(u => u.id == usuarioID);
            if (usuario == null)
            {
                Console.WriteLine("No se encontró el usuario.");
                return false;
            }
            string hashedPassword = HashPass.HashearPass(passwordNueva);
            usuario.password = hashedPassword;
            _contexto.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cambiar la contraseña: {ex.Message}");
            return false;
        }
    }
    public bool CambiarAvatar(int usuarioID, string nuevoAvatar)
    {
        try
        {
            var usuario = _contexto.Usuario.FirstOrDefault(u => u.id == usuarioID);
            if (usuario == null)
            {
                Console.WriteLine("No se encontró el usuario.");
                return false;
            }
            usuario.avatarUrl = nuevoAvatar;
            _contexto.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cambiar el avatar: {ex.Message}");
            return false;
        }
    }



}