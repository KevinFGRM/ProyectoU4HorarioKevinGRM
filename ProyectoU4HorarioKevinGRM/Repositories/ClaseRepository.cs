using Microsoft.Win32;
using ProyectoU4HorarioKevin.Models;
using ProyectoU4HorarioKevin.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoU4HorarioKevin.Repositories
{
    public class ClaseRepository
    {
        SQLiteConnection conexion;

        public ClaseRepository()
        {
            var ruta = "Horario.sqlite";
            conexion = new SQLiteConnection(ruta);
            conexion.CreateTable<ClasesModel>();
        }
        public IEnumerable<ClasesModel> GetAll()
        {
            var datos = conexion.Table<ClasesModel>().OrderBy(x => x.HoraInicio)
                .ToList();
            return datos;
        }
        public void Insert(ClasesModel c)
        {
            conexion.Insert(c);
        }

        public void Delete(ClasesModel c)
        {
            conexion.Delete(c);
        }
        public void Update(ClasesModel c)
        {
            conexion.Update(c);
        }
    }
}
