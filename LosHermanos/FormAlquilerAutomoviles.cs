using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LosHermanos
{
    public partial class FormAlquilerAutomoviles : Form
    {
        private Conexion conexion;
        public FormAlquilerAutomoviles()
        {
            InitializeComponent();
            conexion = new Conexion();
        }

        private void btnAlquilar_Click(object sender, EventArgs e)
        {
            string codigo = txtCodigo.Text;
            // Verificar la condición de la identificación
            string Nombres = comboBoxA.Text;
            string Apellidos = comboBoxC.Text;
            DateTime fechaInicio = dtInicio.Value;
            string CorreoElectronico = txtNit.Text;
            string Contraseña =  txtNombre.Text;

            if (string.IsNullOrEmpty(codigo) || string.IsNullOrEmpty(Nombres) || string.IsNullOrEmpty(Apellidos) ||
        string.IsNullOrEmpty(CorreoElectronico) || string.IsNullOrEmpty(Contraseña))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de realizar el alquiler.");
                return;
            }
            if (!Regex.IsMatch(codigo, @"^AL\d+$"))
            {
                MessageBox.Show("El formato del código de alquiler es incorrecto. Debe comenzar con 'AL' seguido de números.");
                return;
            }

            if (conexion.ExisteCodigo("Alquileres", "CodigoAlquiler", codigo))
            {
                MessageBox.Show("El código ya existe. Por favor, ingrese uno nuevo.");
                return;
            }

            Dictionary<string, object> datos = new Dictionary<string, object>();
                datos.Add("CodigoAlquiler", codigo);
                datos.Add("FechaInicio", fechaInicio);
                datos.Add("CodigoAutomovil", Nombres);
                datos.Add("CodigoCliente", Apellidos);
                datos.Add("NitCliente", CorreoElectronico);
                datos.Add("NombreCliente", Contraseña);

            try
            {
                conexion.InsertarRegistro("Alquileres", datos);

                // Actualizar disponibilidad del automóvil en la tabla Automoviles
                Dictionary<string, object> datosAutomovil = new Dictionary<string, object>();
                datosAutomovil.Add("Disponibilidad", false);

                string condicionAutomovil = "Codigo = @codigo";
                datosAutomovil.Add("codigo", Nombres);

                conexion.ModificarRegistro("Automoviles", condicionAutomovil, datosAutomovil);

                LimpiarCampos();

                MessageBox.Show("Alquiler registrado correctamente.");
                CargarAutomoviles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el registro en la tabla Alquileres: " + ex.Message);
            }

        }

        private void btnBuscarAuto_Click(object sender, EventArgs e)
        {
            string nombre = txtBuscarC.Text;
            string condicion = "Disponibilidad = true AND (Codigo LIKE '%" + nombre + "%' OR Marca LIKE '%" + nombre + "%' OR Modelo LIKE '%" + nombre + "%')";
            DataTable dt = conexion.BuscarRegistros("Automoviles", condicion);
            dataGridViewA.DataSource = dt;
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            string nombre = txtBuscarC.Text;

            DataTable dt = conexion.BuscarRegistros("Clientes", "Codigo LIKE '%" + nombre + "%' OR NIT LIKE '%" + nombre + "%' OR NombreCompleto LIKE '%" + nombre + "%'");
            dataGridViewC.DataSource = dt;
        }

        private void FormAlquilerAutomoviles_Load(object sender, EventArgs e)
        {
            CargarAutomoviles();
            CargarClientes();
        }
        private void LimpiarCampos()
        {
            txtCodigo.Text = string.Empty;
            txtBuscarA.Text = string.Empty;
            txtBuscarC.Text = string.Empty;
            txtNit.Text = string.Empty;
            txtNombre.Text = string.Empty;
            comboBoxC.SelectedIndex = -1;
            comboBoxA.SelectedIndex = -1;
        }
        private void CargarAutomoviles()
        {
            comboBoxA.DisplayMember = "Codigo";
            DataTable dt = conexion.ObtenerAutomovilesDisponibles();
            dataGridViewA.DataSource = dt;
            comboBoxA.DataSource = dt;
            dataGridViewA.Columns["PrecioPorDia"].DefaultCellStyle.Format = "C2";
            dataGridViewA.Columns["PrecioPorDia"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("es-GT");
        }
        private void CargarClientes()
        {
            DataTable dt = conexion.ObtenerRegistros("Clientes");
            comboBoxC.SelectedIndexChanged += ComboBoxC_SelectedIndexChanged;
            dataGridViewC.DataSource = dt;
            comboBoxC.DataSource = dt;
            comboBoxC.DisplayMember = "Codigo";
        }
        private void ComboBoxC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxC.SelectedItem != null)
            {
                DataRowView selectedClient = comboBoxC.SelectedItem as DataRowView;
                string nit = selectedClient["NIT"].ToString();
                string nombre = selectedClient["NombreCompleto"].ToString();

                txtNit.Text = nit;
                txtNombre.Text = nombre;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormMenuPrincipal formMenuPrincipal = new FormMenuPrincipal();
            formMenuPrincipal.ShowDialog();
            this.Close();
        }
    }
}
