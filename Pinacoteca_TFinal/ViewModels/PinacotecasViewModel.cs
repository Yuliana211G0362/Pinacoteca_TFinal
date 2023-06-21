using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Pinacoteca_TFinal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pinacoteca_TFinal.ViewModels
{
    public class PinacotecasViewModel : INotifyPropertyChanged
    {
        public PinacotecasViewModel()
        {
            Cargar();
            AgregarCommand = new RelayCommand(Agregar);
            EditarCommand = new RelayCommand(Editar);
            EliminarCommand = new RelayCommand(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            CambiarVistaCommand = new RelayCommand<string>(CambiarVista);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        int indice;

        public string Error { get; set; } = "";
        public ObservableCollection<Pinacoteca> Pinacotecas { get; set; } = new ObservableCollection<Pinacoteca>();
        public Pinacoteca pinacoteca { get; set; }

        public Pinacoteca Pinacoteca
        {
            get { return pinacoteca; }
            set
            {
                pinacoteca = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
            }
        }


        public string Vista { get; set; } = "ver";
        public ICommand? AgregarCommand { get; set; }
        public ICommand? EditarCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }
        public ICommand? CambiarVistaCommand { get; set; }
        public ICommand? CancelarCommand { get; set; }


        public void Agregar()
        {
            Error = "";


            for (int i = 0; i < Pinacotecas.Count; i++)
            {
                if (Pinacotecas[i].Nombre==Pinacoteca.Nombre)
                {
                    Error += "Ya fue agregada";
                }
            }




            if (string.IsNullOrEmpty(Pinacoteca.Nombre))
            {
                Error += "Escriba el nombre de la pinacoteca\n";
            }
            if (string.IsNullOrEmpty(Pinacoteca.Ciudad))
            {
                Error += "Escriba la Ciudad\n";
            }
            if (string.IsNullOrEmpty(Pinacoteca.Direccion))
            {
                Error += "Escriba la direccion\n";
            }
            if (string.IsNullOrEmpty(Pinacoteca.Area))
            {
                Error += "Escriba el Area\n";
            }



            if (Error == "" && Pinacotecas != null)
            {
                Pinacotecas.Add(Pinacoteca);
                Guardar();
                Vista = "ver";
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        }

        public void CambiarVista(string vistaACambiar)
        {
            Vista = vistaACambiar;

            if (vistaACambiar == "agregar")
            {
                pinacoteca = new Pinacoteca();
            }
            if (vistaACambiar == "editar")
            {
                if (Pinacotecas != null)
                {
                    indice = Pinacotecas.IndexOf(Pinacoteca);
                }
                Pinacoteca copiapinacoteca = new Pinacoteca()
                {
                    Nombre = Pinacoteca.Nombre,
                    Ciudad = Pinacoteca.Ciudad,
                    Direccion = Pinacoteca.Direccion,
                    Area = Pinacoteca.Area
                };
                Pinacoteca = copiapinacoteca;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Vista"));

        }

        public void Cancelar()
        {
            Vista = "ver";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
        }


        public void Editar()
        {
            if (Pinacotecas != null)
            {
                Pinacotecas[indice] = Pinacoteca;
                Guardar();
                CambiarVista("ver");

            }

        }

        public void Eliminar()
        {
            if (Pinacoteca != null && Pinacotecas != null)
            {
                Pinacotecas.Remove(Pinacoteca);
                CambiarVista("ver");
                Guardar();
            }
        }

        public void Guardar()
        {
            var json = JsonConvert.SerializeObject(Pinacotecas);
            File.WriteAllText("Libros.json", json);

        }

        public void Cargar()
        {
            if (File.Exists("Libros.json"))
            {
                var json = File.ReadAllText("Libros.json");
                var datos = JsonConvert.DeserializeObject<ObservableCollection<Pinacoteca>>(json);
                if (datos != null)
                {
                    Pinacotecas = (ObservableCollection<Pinacoteca>)datos;
                }
                else
                {
                    Pinacotecas = new ObservableCollection<Pinacoteca>();

                }
            }
        }


    }
}