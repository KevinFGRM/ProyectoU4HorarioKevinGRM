using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU4HorarioKevin.Models
{
    public enum Dia { Lunes, Martes, Miercoles, Jueves, Viernes }
    [Table("ClasesT")]
    public class ClasesModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Nombre { get; set; } = "";
        [NotNull]
        public string Maestro { get; set; } = null!;
        [NotNull]
        public string Aula { get; set; } = "";
        [NotNull]
        public string Descripcion { get; set; } = "";
        [NotNull]
        public string Tipo { get; set; } = null!;
        [NotNull]
        public Dia DiaSemana { get; set; }
        [NotNull]
        public DateTime HoraInicio { get; set; }
        [NotNull]
        public DateTime HoraFin { get; set; }
    }
}
