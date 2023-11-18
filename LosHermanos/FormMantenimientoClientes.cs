﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LosHermanos
{
    public partial class FormMantenimientoClientes : Form
    {
        private Conexion conexion;

        public FormMantenimientoClientes()
        {
            InitializeComponent();
            conexion = new Conexion();
        }

        private void CargarClientes()
        {
            DataTable dt = conexion.ObtenerRegistros("Clientes");
            dataGridView1.DataSource = dt;
        }


        private void LimpiarCampos()
        {
            txtCodigo.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtModelo.Text = string.Empty;
            txtAnioFabricacion.Text = string.Empty;
            txtPrecioPorDia.Text = string.Empty;
            txtNit.Text = string.Empty;
        }

        private void FormMantenimientoClientes_Load_1(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo.Text;
            // Verificar la condición de la identificación
            if (!codigo.StartsWith("C") || !codigo.Substring(1).All(char.IsDigit))
            {
                MessageBox.Show("El código debe comenzar con 'C' seguido de números.");
                return;
            }
            string NombreCompleto = txtMarca.Text;
            string Direccion = txtModelo.Text;
            string NumeroTelefono = txtAnioFabricacion.Text;
            string CorreoElectronico = txtPrecioPorDia.Text;
            string Nit = txtNit.Text;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Codigo"].Value != null && row.Cells["Codigo"].Value.ToString() == codigo)
                {
                    MessageBox.Show("El código ya existe. Por favor, ingrese uno nuevo.");
                    return;
                }
            }

            Dictionary<string, object> datos = new Dictionary<string, object>();
            datos.Add("Codigo", codigo);
            datos.Add("NombreCompleto", NombreCompleto);
            datos.Add("Direccion", Direccion);
            datos.Add("NumeroTelefono", NumeroTelefono);
            datos.Add("CorreoElectronico", CorreoElectronico);
            datos.Add("NIT", Nit);

            conexion.InsertarRegistro("Clientes", datos);

            LimpiarCampos();
            CargarClientes();
                MessageBox.Show("El cliente se ha insertado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el cliente: " + ex.Message);
            }
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = txtCodigo.Text;
                string NombreCompleto = txtMarca.Text;
                string Direccion = txtModelo.Text;
                string NumeroTelefono = txtAnioFabricacion.Text;
                string CorreoElectronico = txtPrecioPorDia.Text;
                string Nit = txtNit.Text;

                Dictionary<string, object> datos = new Dictionary<string, object>();
                datos.Add("NombreCompleto", NombreCompleto);
                datos.Add("Direccion", Direccion);
                datos.Add("NumeroTelefono", NumeroTelefono);
                datos.Add("CorreoElectronico", CorreoElectronico);
                datos.Add("NIT", Nit);

                string condicion = "Codigo = '" + codigo + "'";

                conexion.ModificarRegistro("Clientes", condicion, datos);
                MessageBox.Show("El cliente se ha modificado correctamente.");

                LimpiarCampos();
                CargarClientes();
            }
            else
            {
                MessageBox.Show("Selecciona un cliente para modificar.", "Información");
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = txtCodigo.Text;

                string condicion = "Codigo = '" + codigo + "'";

                conexion.EliminarRegistro("Clientes", condicion);
                MessageBox.Show("El clinete se ha eliminado correctamente.");

                LimpiarCampos();
                CargarClientes();
            }
            else
            {
                MessageBox.Show("Selecciona un cliente para eliminar.", "Información");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombre = txtBuscar.Text;

            DataTable dt = conexion.BuscarRegistros("Clientes", "Codigo LIKE '%" + nombre + "%' OR NIT LIKE '%" + nombre + "%' OR NombreCompleto LIKE '%" + nombre + "%'");
            dataGridView1.DataSource = dt;
        }

        private void btnTraerDatos_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = dataGridView1.SelectedRows[0].Cells["Codigo"].Value.ToString();

                conexion.AbrirConexion();

                string query = "SELECT Codigo, NombreCompleto, Direccion, NumeroTelefono, CorreoElectronico, Nit " +
                               "FROM Clientes WHERE Codigo = @codigo";

                OleDbCommand command = new OleDbCommand(query, conexion.con);
                command.Parameters.AddWithValue("@codigo", codigo);

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    txtCodigo.Text = dataTable.Rows[0]["Codigo"].ToString();
                    txtMarca.Text = dataTable.Rows[0]["NombreCompleto"].ToString();
                    txtModelo.Text = dataTable.Rows[0]["Direccion"].ToString();
                    txtAnioFabricacion.Text = dataTable.Rows[0]["NumeroTelefono"].ToString();
                    txtPrecioPorDia.Text = dataTable.Rows[0]["CorreoElectronico"].ToString();
                    txtNit.Text = dataTable.Rows[0]["Nit"].ToString();
                }
                else
                {
                    MessageBox.Show("No se encontraron datos del cliente seleccionado.");
                }

                conexion.CerrarConexion();
            }
            else
            {
                MessageBox.Show("Seleccione un cliente para traer los datos.");
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
