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
    public partial class FormListadoClientes : Form
    {
        private Conexion conexion;
        public FormListadoClientes()
        {
            InitializeComponent();
            conexion = new Conexion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txt_buscar.Text;

                // Abre la conexión a la base de datos
                conexion.AbrirConexion();

                // Consulta para buscar clientes por nombre o NIT
                string query = "SELECT Codigo, NombreCompleto, Direccion, NIT, NumeroTelefono, CorreoElectronico FROM clientes WHERE NombreCompleto LIKE '%" + nombre + "%' OR NIT LIKE '%" + nombre + "%'";

                // Crea un adaptador de datos y un DataSet
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conexion.con);
                DataSet dataSet = new DataSet();

                // Llena el DataSet con los datos obtenidos de la consulta
                adapter.Fill(dataSet, "Clientes");

                // Asigna los datos al DataGridView
                dataGridView1.DataSource = dataSet.Tables["Clientes"];

                // Cierra la conexión a la base de datos
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar los clientes: " + ex.Message);
            }
        }
        

        private void FormListadoClientes_Load(object sender, EventArgs e)
        {
            try
            {
                // Abre la conexión a la base de datos
                conexion.AbrirConexion();

                // Consulta para obtener los clientes
                string query = "SELECT Codigo, NombreCompleto, Direccion, NumeroTelefono, CorreoElectronico, NIT FROM clientes";

                // Crea un adaptador de datos y un DataSet
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conexion.con);
                DataSet dataSet = new DataSet();

                // Llena el DataSet con los datos obtenidos de la consulta
                adapter.Fill(dataSet, "Clientes");

                // Asigna los datos al DataGridView
                dataGridView1.DataSource = dataSet.Tables["Clientes"];

                // Cierra la conexión a la base de datos
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar los clientes: " + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            FormMenuPrincipal formMenuPrincipal = new FormMenuPrincipal();
            formMenuPrincipal.ShowDialog();
            this.Close();
        }
    }
}
