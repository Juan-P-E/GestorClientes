using System;

namespace GestorClientes
{
    public class Cliente
    {
        public int Id { get; set; }  // Identificador único
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Empresa { get; set; }
        public DateTime FechaAlta { get; set; }
        
        

        public override string ToString()
        {
            return $"[{Id}] {Nombre} - {Email} - {Telefono} - {Empresa} - Alta: {FechaAlta.ToShortDateString()}";
        }
    }
}
