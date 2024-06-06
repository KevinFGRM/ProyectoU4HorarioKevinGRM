using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using ProyectoU4HorarioKevin.Models;
using ProyectoU4HorarioKevin.Repositories;
using ProyectoU4HorarioKevin.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoU4HorarioKevin.ViewModels
{
    public enum Dias { Lunes, Martes, Miercoles, Jueves, Viernes }
    public enum Vistas { Clases, AgregarC, EditarC, EliminarC, AgregarA, EditarA }
    public class HorarioViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ClasesModel> Clases { get; set; } = new ObservableCollection<ClasesModel>();
        public string Error { get; set; } = "";
        ClaseRepository repository = new();
        public ClasesModel? Clase { get; set; }
        public Vistas Vista { get; set; } = Vistas.Clases;
        public ObservableCollection<Dias> DiasSemana { get; set; } = new ObservableCollection<Dias>();

        private Dias diaSeleccionado;
        public Dias DiaSeleccionado
        {
            get => diaSeleccionado;
            set
            {
                diaSeleccionado = value;
                Actualizar(nameof(DiaSeleccionado));
                Actualizar(nameof(ClasesFiltradas));
            }
        }
        public IEnumerable<ClasesModel> ClasesFiltradas
        {
            get
            {
                return Clases.Where(c => (Dias)c.DiaSemana == DiaSeleccionado);
            }
        }

        private ClasesModel claseseleccionada;
        public ClasesModel ClaseSeleccionada
        {
            get => claseseleccionada;
            set
            {
                claseseleccionada = value;
                Actualizar();
            }
        }

        public ICommand AgregarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand CambiarVistaCommand { get; set; }


        public HorarioViewModel()
        {
            DiasSemana.Add(Dias.Lunes);
            DiasSemana.Add(Dias.Martes);
            DiasSemana.Add(Dias.Miercoles);
            DiasSemana.Add(Dias.Jueves);
            DiasSemana.Add(Dias.Viernes);

            CancelarCommand = new RelayCommand(Cancelar);
            AgregarCommand = new RelayCommand(AgregarClase);
            EliminarCommand = new RelayCommand(EliminarClase);
            GuardarCommand = new RelayCommand(GuardarClase);
            CambiarVistaCommand = new RelayCommand<Vistas>(CambiarVista);
            DiaSeleccionado = Dias.Lunes;
            foreach (var clase in repository.GetAll())
            {
                Clases.Add(clase);
            }
        }

        private void Cancelar()
        {
            CambiarVista(Vistas.Clases);
        }

        private void CambiarVista(Vistas vistas)
        {
            Vista = vistas;
            if (Vista == Vistas.AgregarC)
            {
                Clase = new();
                Clase.Tipo = "Clase";
                Actualizar(nameof(Vista));
            }
            if (Vista == Vistas.AgregarA)
            {
                Clase = new();
                Clase.Tipo = "Actividad";
                Actualizar(nameof(Vista));
            }
            if (Vista == Vistas.EditarC && Clase != null)
            {
                ClaseSeleccionada = Clase;
                if (ClaseSeleccionada.Tipo == "Actividad")
                {
                    Vista = Vistas.Clases;
                }
            }
            if (Vista == Vistas.EditarA && Clase != null)
            {
                ClaseSeleccionada = Clase;
                if (ClaseSeleccionada.Tipo == "Clase")
                {
                    Vista = Vistas.Clases;
                }
            }
            Error = "";
            Actualizar(nameof(Clase));
            Actualizar(nameof(Error));
            Actualizar(nameof(Vista));
        }


        private void EliminarClase()
        {
            if(Clase != null)
            {
                repository.Delete(Clase);
                Clases.Remove(Clase);
                CambiarVista(Vistas.Clases);
                Actualizar(nameof(ClasesFiltradas));

            }
        }

        private void AgregarClase()
        {
            Error = "";
            if (Clase != null)
            {
                Clase.DiaSemana = (Dia)DiaSeleccionado;
                if (Clase.Tipo == "Clase")
                {
                    if (Clase.HoraInicio >= Clase.HoraFin)
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (Clases.Any(c =>
                        ((c.HoraInicio <= Clase.HoraInicio && c.HoraFin > Clase.HoraInicio) ||
                        (c.HoraInicio < Clase.HoraFin && c.HoraFin >= Clase.HoraFin) ||
                        (c.HoraInicio >= Clase.HoraInicio && c.HoraFin <= Clase.HoraFin))
                        && c.DiaSemana == Clase.DiaSemana))
                    {
                        Error = "Ya hay una clase programada en ese horario.";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.Aula))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.Maestro))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.Nombre))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (Error == "")
                    {
                        repository.Insert(Clase);
                        Clases.Insert(0, Clase);
                        CambiarVista(Vistas.Clases);
                    }
                    Actualizar(nameof(Error));
                }
                else
                {
                    if (Clase.HoraInicio >= Clase.HoraFin)
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (Clases.Any(c =>
                        ((c.HoraInicio <= Clase.HoraInicio && c.HoraFin > Clase.HoraInicio) ||
                        (c.HoraInicio < Clase.HoraFin && c.HoraFin >= Clase.HoraFin) ||
                        (c.HoraInicio >= Clase.HoraInicio && c.HoraFin <= Clase.HoraFin))
                        && c.DiaSemana == Clase.DiaSemana))
                    {
                        Error = "Ya hay una clase programada en ese horario.";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.Maestro))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (string.IsNullOrWhiteSpace(Clase.Descripcion))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (Error == "")
                    {
                        repository.Insert(Clase);
                        Clases.Insert(0, Clase);
                        CambiarVista(Vistas.Clases);
                    }
                    Actualizar(nameof(Error));
                }
                Actualizar(nameof(ClasesFiltradas));
            }
        }

        private void GuardarClase()
        {
            Error = "";
            if (ClaseSeleccionada != null)
            {
                if (ClaseSeleccionada.Tipo == "Clase")
                {
                    if (ClaseSeleccionada.HoraInicio >= ClaseSeleccionada.HoraFin)
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (Clases.Any(c =>
                        ((c.HoraInicio <= ClaseSeleccionada.HoraInicio && c.HoraFin > ClaseSeleccionada.HoraInicio) ||
                        (c.HoraInicio < ClaseSeleccionada.HoraFin && c.HoraFin >= ClaseSeleccionada.HoraFin) ||
                        (c.HoraInicio >= ClaseSeleccionada.HoraInicio && c.HoraFin <= ClaseSeleccionada.HoraFin))
                        && c.DiaSemana == ClaseSeleccionada.DiaSemana && c.Id != ClaseSeleccionada.Id))
                    {
                        Error = "Ya hay una clase programada en ese horario.";
                    }
                    if (string.IsNullOrWhiteSpace(ClaseSeleccionada.Aula))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (string.IsNullOrWhiteSpace(ClaseSeleccionada.Maestro))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (string.IsNullOrWhiteSpace(ClaseSeleccionada.Nombre))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (Error == "")
                    {
                        repository.Update(ClaseSeleccionada);
                        CambiarVista(Vistas.Clases);
                    }
                    Actualizar(nameof(Error));
                }
                else
                {
                    if (ClaseSeleccionada.HoraInicio >= ClaseSeleccionada.HoraFin)
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (Clases.Any(c =>
                        ((c.HoraInicio <= ClaseSeleccionada.HoraInicio && c.HoraFin > ClaseSeleccionada.HoraInicio) ||
                        (c.HoraInicio < ClaseSeleccionada.HoraFin && c.HoraFin >= ClaseSeleccionada.HoraFin) ||
                        (c.HoraInicio >= ClaseSeleccionada.HoraInicio && c.HoraFin <= ClaseSeleccionada.HoraFin))
                        && c.DiaSemana == ClaseSeleccionada.DiaSemana && c.Id != ClaseSeleccionada.Id))
                    {
                        Error = "Ya hay una clase programada en ese horario.";
                    }
                    if (string.IsNullOrWhiteSpace(ClaseSeleccionada.Maestro))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (string.IsNullOrWhiteSpace(ClaseSeleccionada.Descripcion))
                    {
                        Error = "Verifique los datos ingresados.";
                    }
                    if (Error == "")
                    {
                        repository.Update(ClaseSeleccionada);
                        CambiarVista(Vistas.Clases);
                    }
                    Actualizar(nameof(Error));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Actualizar(string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
