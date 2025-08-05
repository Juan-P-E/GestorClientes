using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GestorClientes
{
    class Program
    {
        static List<Cliente> clientes = new(); // Lista para guardar los clientes
        static int siguienteId = 1; // Para asignar IDs automáticos
        static string archivoClientes = "clientes.json"; // Nombre del archivo

        static void Main(string[] args)
        {
            CargarClientes(); // Leer del archivo al iniciar

            while (true)
            {
                MostrarMenu();
                var opcion = Console.ReadLine();

                switch (opcion)

                {
                    case "1":
                        AgregarCliente();
                        break;
                    case "2":
                        ListarClientes();
                        break;
                    case "3":
                        EliminarCliente();

                        break;
                    case "4":
                        ModificarCliente();

                        
                        break;
                    case "5":
                        BuscarCliente();

                        break;
                    case "0":
                        Console.WriteLine("¡Hasta luego!");
                        return;
                    default:
                        Console.WriteLine("Opción inválida. Intente de nuevo.");
                        break;
                }
            }
        }


        static void ModificarCliente()
        {
            Console.Write("Ingrese el ID del cliente a modificar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var cliente = clientes.Find(c => c.Id == id);
                if (cliente != null)
                {
                    Console.WriteLine($"Cliente actual: {cliente.Nombre} | {cliente.Email} | {cliente.Telefono}");

                    Console.Write("Nuevo nombre (Enter para mantener): ");
                    string? nuevoNombre = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevoNombre))
                        cliente.Nombre = nuevoNombre;

                    Console.Write("Nuevo email (Enter para mantener): ");
                    string? nuevoEmail = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevoEmail))
                        cliente.Email = nuevoEmail;

                    Console.Write("Nuevo teléfono (Enter para mantener): ");
                    string? nuevoTelefono = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevoTelefono))
                        cliente.Telefono = nuevoTelefono;

                    GuardarClientes();
                    Console.WriteLine("Cliente modificado correctamente.");
                }
                else
                {
                    Console.WriteLine("No se encontró un cliente con ese ID.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
        }
        static void MostrarMenu()
        {
            Console.WriteLine("\n--- Gestor de Clientes ---");
            Console.WriteLine("1. Agregar cliente");
            Console.WriteLine("2. Listar clientes");
            Console.WriteLine("3. Eliminar cliente");
            Console.WriteLine("4. Modificar cliente");
            Console.WriteLine("5. Buscar cliente");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");
        }

        static void BuscarCliente()
        {
            Console.Write("Ingrese nombre o email a buscar: ");
            string? entrada = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(entrada))
            {
                Console.WriteLine("Búsqueda vacía, intente nuevamente.");
                return;
            }

            string criterio = entrada.ToLower();

            var resultados = clientes.FindAll(c =>
                (!string.IsNullOrEmpty(c.Nombre) && c.Nombre.ToLower().Contains(criterio)) ||
                (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(criterio))
            );

            if (resultados.Count > 0)
            {
                Console.WriteLine("\nResultados encontrados:");
                foreach (var cliente in resultados)
                {
                    Console.WriteLine($"ID: {cliente.Id} | Nombre: {cliente.Nombre} | Email: {cliente.Email} | Tel: {cliente.Telefono}");
                }
            }
            else
            {
                Console.WriteLine("No se encontraron clientes que coincidan con la búsqueda.");
            }
        }


        static void AgregarCliente()
        {
            Console.WriteLine("\n--- Agregar Cliente ---");

            Console.Write("Nombre: ");
            string? nombre = Console.ReadLine();

            Console.Write("Email: ");
            string? email = Console.ReadLine();

            Console.Write("Teléfono: ");
            string? telefono = Console.ReadLine();

            Console.Write("Empresa: ");
            string? empresa = Console.ReadLine();

            var cliente = new Cliente
            {
                Id = siguienteId++,
                Nombre = nombre,
                Email = email,
                Telefono = telefono,
                Empresa = empresa,
                FechaAlta = DateTime.Now
            };

            clientes.Add(cliente);
            GuardarClientes();

            Console.WriteLine("✅ Cliente agregado correctamente.");
        }

        static void EliminarCliente()
        {
            Console.Write("Ingrese el ID del cliente a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var cliente = clientes.Find(c => c.Id == id);
                if (cliente != null)
                {
                    clientes.Remove(cliente);
                    Console.WriteLine($"Cliente {cliente.Nombre} eliminado correctamente.");
                    GuardarClientes();
                }
                else
                {
                    Console.WriteLine("No se encontró un cliente con ese ID.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
        }

        static void ListarClientes()
        {
            Console.WriteLine("\n--- Lista de Clientes ---");

            if (clientes.Count == 0)
            {
                Console.WriteLine("No hay clientes cargados.");
                return;
            }

            foreach (var cliente in clientes)
            {
                Console.WriteLine(cliente);
            }
        }

        static void GuardarClientes()
        {
            string json = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(archivoClientes, json);
        }


        static void CargarClientes()
        {
            if (File.Exists(archivoClientes))
            {
                string json = File.ReadAllText(archivoClientes);
                var lista = JsonSerializer.Deserialize<List<Cliente>>(json);

                if (lista != null)
                {
                    clientes = lista;
                    if (clientes.Count > 0)
                        siguienteId = clientes[^1].Id + 1; // "^1" es el último elemento
                }
            }
        }
    }
}
