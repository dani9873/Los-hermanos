using System;
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
    public partial class Form1 : Form
    {
        private Conexion conexion;

        public Form1()
        {
            InitializeComponent();
            conexion = new Conexion();
        }
    
        private void btnIniciarSesion_Click_1(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string contraseña = txtContraseña.Text;

            if (usuario == "" || contraseña == "")
            {
                MessageBox.Show("Llene todos los campos");
                return;
            }

            try
            {
                conexion.AbrirConexion();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Usuarios WHERE NombreUsuario = '" + usuario + "' AND Contraseña = '" + contraseña + "'", conexion.con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Bienvenido");
                    FormMenuPrincipal formMenuPrincipal = new FormMenuPrincipal();
                    formMenuPrincipal.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error de inicio");
                }

                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar la consulta: " + ex.Message);
            }
        }
    }
}
