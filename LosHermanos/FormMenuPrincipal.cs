using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LosHermanos
{
    public partial class FormMenuPrincipal : Form
    {
        public FormMenuPrincipal()
        {
            InitializeComponent();
        }
        private void btnMantenimientoAutomoviles_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormMantenimientoAutomoviles formAutomoviles = new FormMantenimientoAutomoviles();
            formAutomoviles.Show();
            this.Close();
        }

        private void btnMantenimientoClientes_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormMantenimientoClientes formClientes = new FormMantenimientoClientes();
            formClientes.Show();
            this.Close();
        }

        private void btnMantenimientoSucursales_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormMantenimientoSucursales formSucursales = new FormMantenimientoSucursales();
            formSucursales.Show();
            this.Close();
        }

        private void btnMantenimientoUsuarios_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormMantenimientoUsuarios formMantenimientoUsuarios = new FormMantenimientoUsuarios();
            formMantenimientoUsuarios.ShowDialog();
            this.Close();
        }

        private void btnAlquilerAutomoviles_Click_1(object sender, EventArgs e)
        {
                this.Hide();
            FormAlquilerAutomoviles formAlquilerAutomoviles = new FormAlquilerAutomoviles();
            formAlquilerAutomoviles.ShowDialog();
            this.Close();
        }

        private void btnDevolucionAutomoviles_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormDevolucionAutomoviles formDevolucionAutomoviles = new FormDevolucionAutomoviles();
            formDevolucionAutomoviles.ShowDialog();
                this.Close();
        }

        private void btnListadoAutomoviles_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormListadoAutomoviles formListadoAutomoviles = new FormListadoAutomoviles();
            formListadoAutomoviles.ShowDialog();
            this.Close();
        }

        private void btnListadoClientes_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormListadoClientes formListadoClientes = new FormListadoClientes();
            formListadoClientes.ShowDialog();
            this.Close();
        }

        private void btnListadoSucursales_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormListadoSucursales formListadoSucursales = new FormListadoSucursales();
            formListadoSucursales.ShowDialog();
            this.Close();
        }

        private void btnListadoUsuarios_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormListadoUsuarios formListadoUsuarios = new FormListadoUsuarios();
            formListadoUsuarios.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close(); // Cierra la ventana actual
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void btnListadoAlquileres_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormListadoAlquileres formListadoAlquileres = new FormListadoAlquileres();
            formListadoAlquileres.ShowDialog();
            this.Close();
        }
    }
}


