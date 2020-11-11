using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tcc_windows_version.Business;
using tcc_windows_version.Database;
using tcc_windows_version.Model;
using tcc_windows_version.Properties;

namespace tcc_windows_version
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        string idDespesaSelecionada;
        string idReceitaSelecionada;
        string nome_usuario;
        int idUsuario;

        #region Borda Customizada
        static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }
            return (IntPtr)0;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
            if (monitor != IntPtr.Zero)
            {
                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }
            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>x coordinate of point.</summary>
            public int x;
            /// <summary>y coordinate of point.</summary>
            public int y;
            /// <summary>Construct a point of coordinates (x,y).</summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public static readonly RECT Empty = new RECT();
            public int Width { get { return Math.Abs(right - left); } }
            public int Height { get { return bottom - top; } }
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            public RECT(RECT rcSrc)
            {
                left = rcSrc.left;
                top = rcSrc.top;
                right = rcSrc.right;
                bottom = rcSrc.bottom;
            }
            public bool IsEmpty { get { return left >= right || top >= bottom; } }
            public override string ToString()
            {
                if (this == Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }
            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode() => left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2) { return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom); }
            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2) { return !(rect1 == rect2); }
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Settings.Default.Reload();

            //Para arrumar a data que aparece no DataGrid
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            //FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            //Para retirar a barra superior padrão do windows e permitir mover o programa sem ela
            SourceInitialized += (s, e) =>
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };

            gdDespesas.Visibility = Visibility.Visible;
            gdFiltrar.Visibility = Visibility.Hidden;
            gdReceitas.Visibility = Visibility.Hidden;

            dgReceitas.Visibility = Visibility.Hidden;
            dgDespesas.Visibility = Visibility.Visible;

            UsuarioBO bo = new UsuarioBO();
            Usuario usuario = new Usuario();
            usuario.email = Settings.Default["email"].ToString();
            usuario.senha = Settings.Default["senha"].ToString();
            idUsuario = bo.Logar(usuario);
            nome_usuario = bo.nome_usuario;

            lblNomeUsuario.Content = "Olá, " + nome_usuario;

            atualizarGridDespesasAnoAtual();
        }

        public void atualizarGridDespesasAnoAtual()
        {
            try
            {
                /*MySqlConnection objSqlConnnection = new MySqlConnection("server=localhost;port=3306;User Id=root;password=;database=sym");
                MySqlDataAdapter adpt;
                DataTable data = new DataTable("despesas");

                objSqlConnnection.Open();
                adpt = new MySqlDataAdapter("select estado, nome, empresa, categoria, valor, data_vencimento from despesas", objSqlConnnection.ConnectionString);
                adpt.Fill(data);
                dgDespesas.ItemsSource = data.DefaultView;*/


                Connection objConexao = new Connection();
                MySqlDataAdapter adpt;
                DataTable data = new DataTable("despesas");

                string anoAtual = DateTime.Now.Year.ToString();
                adpt = new MySqlDataAdapter("select * from despesas where id_usuario = '" + idUsuario + "' and YEAR(data_vencimento) = " + anoAtual + ";", objConexao.Conexao());
                adpt.Fill(data);
                dgDespesas.ItemsSource = data.DefaultView;

                objConexao.Desconectar();
            }
            catch
            {
                MessageBox.Show("Não foi possível conectar com o banco de dados.");
            }
        }
        public void atualizarGridReceitasAnoAtual()
        {
            try
            {
                Connection objConexao = new Connection();
                MySqlDataAdapter adpt;
                DataTable data = new DataTable("receitas");

                string anoAtual = DateTime.Now.Year.ToString();
                adpt = new MySqlDataAdapter("select * from receitas where id_usuario = '" + idUsuario + "' and YEAR(data_insercao) = " + anoAtual + ";", objConexao.Conexao());
                adpt.Fill(data);
                dgReceitas.ItemsSource = data.DefaultView;

                objConexao.Desconectar();
            }
            catch
            {
                MessageBox.Show("Não foi possível conectar com o banco de dados.");
            }
        }
        private void dgDespesas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView row_selected = dg.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                idDespesaSelecionada = row_selected["id"].ToString();
                cbEstadoDespesas.Text = row_selected["estado"].ToString();
                txtNome.Text = row_selected["nome"].ToString();
                txtEmpresa.Text = row_selected["empresa"].ToString();
                cbCategoriaDespesas.Text = row_selected["categoria"].ToString();
                txtValorDespesas.Text = row_selected["valor"].ToString();
                dpDespesa.Text = row_selected["data_vencimento"].ToString();
            }
        }
        private void dgReceitas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            DataRowView row_selected = dg.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                idReceitaSelecionada = row_selected["id"].ToString();               
                txtDescricaoReceita.Text = row_selected["descricao"].ToString();                
                cbCategoriaReceita.Text = row_selected["categoria"].ToString();
                txtValorReceita.Text = row_selected["valor"].ToString();                
            }
        }

        #region Botões laterais
        private void btnDespesas_Click(object sender, RoutedEventArgs e)
        {
            gdDespesas.Visibility = Visibility.Visible;
            gdFiltrar.Visibility = Visibility.Hidden;
            gdReceitas.Visibility = Visibility.Hidden;
            gdLateralBotoes.Margin = new Thickness(-1,125,0,0);

            dgReceitas.Visibility = Visibility.Hidden;
            dgDespesas.Visibility = Visibility.Visible;

            atualizarGridDespesasAnoAtual();
        }

        private void btnReceitas_Click(object sender, RoutedEventArgs e)
        {
            gdDespesas.Visibility = Visibility.Hidden;
            gdFiltrar.Visibility = Visibility.Hidden;
            gdReceitas.Visibility = Visibility.Visible;
            gdLateralBotoes.Margin = new Thickness(-1, 190, 0, 0);
            dgDespesas.Visibility = Visibility.Hidden;
            dgReceitas.Visibility = Visibility.Visible;

            atualizarGridReceitasAnoAtual();
        }

        private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            gdFiltrar.Visibility = Visibility.Visible;
            gdReceitas.Visibility = Visibility.Hidden;
            gdDespesas.Visibility = Visibility.Hidden;
            gdLateralBotoes.Margin = new Thickness(-1, 255, 0, 0);

            dgReceitas.Visibility = Visibility.Hidden;
            dgDespesas.Visibility = Visibility.Visible;

            atualizarGridDespesasAnoAtual();
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            gdLateralBotoes.Margin = new Thickness(-1, 488, 0, 0);
        }
        #endregion

        #region Botões borda windows
        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximizar_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            } else
            {
                WindowState = WindowState.Maximized;
            }            
        }

        private void btnFechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Botões despesa
        private void btnCadastrarDespesas_Click(object sender, RoutedEventArgs e)
        {
            Despesas despesa = new Despesas();
            DespesasBO bo = new DespesasBO();

            despesa.estado = cbEstadoDespesas.Text.ToString();
            despesa.nome = txtNome.Text.ToString();
            despesa.categoria = cbCategoriaDespesas.Text.ToString();
            despesa.empresa = txtEmpresa.Text.ToString();
            despesa.valor = txtValorDespesas.Text;
            despesa.data_vencimento = DateTime.Parse(dpDespesa.Text).ToString("yyyy-MM-dd");
            despesa.id_usuario = idUsuario;

            bo.Cadastrar(despesa);

            atualizarGridDespesasAnoAtual();

            idDespesaSelecionada = "";
            cbEstadoDespesas.SelectedIndex = -1;
            txtNome.Text = "";
            cbCategoriaDespesas.SelectedIndex = -1;
            txtEmpresa.Text = "";
            txtValorDespesas.Text = "";
            dpDespesa.Text = "";
        }

        private void btnAlterarDespesas_Click(object sender, RoutedEventArgs e)
        {
            Despesas despesa = new Despesas();
            DespesasBO bo = new DespesasBO();

            despesa.id = idDespesaSelecionada;
            despesa.estado = cbEstadoDespesas.Text.ToString();
            despesa.nome = txtNome.Text.ToString();
            despesa.categoria = cbCategoriaDespesas.Text.ToString();
            despesa.empresa = txtEmpresa.Text.ToString();
            despesa.valor = txtValorDespesas.Text.ToString();
            despesa.data_vencimento = DateTime.Parse(dpDespesa.Text).ToString("yyyy-MM-dd");
            despesa.id_usuario = idUsuario;

            bo.Editar(despesa);

            atualizarGridDespesasAnoAtual();

            idDespesaSelecionada = "";
            cbEstadoDespesas.SelectedIndex = -1;
            txtNome.Text = "";
            cbCategoriaDespesas.SelectedIndex = -1;
            txtEmpresa.Text = "";
            txtValorDespesas.Text = "";
            dpDespesa.Text = "";
        }

        private void btnDeletarDespesas_Click(object sender, RoutedEventArgs e)
        {
            DespesasBO despesa = new DespesasBO();
            despesa.Deletar(Convert.ToInt32(idDespesaSelecionada));

            atualizarGridDespesasAnoAtual();

            idDespesaSelecionada = "";
            cbEstadoDespesas.SelectedIndex = -1;
            txtNome.Text = "";
            cbCategoriaDespesas.SelectedIndex = -1;
            txtEmpresa.Text = "";
            txtValorDespesas.Text = "";
            dpDespesa.Text = "";
        }

        private void btnLimparDespesas_Click(object sender, RoutedEventArgs e)
        {
            idDespesaSelecionada = "";
            cbEstadoDespesas.SelectedIndex = -1;
            txtNome.Text = "";
            cbCategoriaDespesas.SelectedIndex = -1;
            txtEmpresa.Text = "";
            txtValorDespesas.Text = "";
            dpDespesa.Text = "";
        }
        #endregion

        #region Botões receita
        private void btnAdicionarReceitas_Click(object sender, RoutedEventArgs e)
        {
            Receitas receita = new Receitas();
            ReceitasBO bo = new ReceitasBO();

            receita.descricao = txtDescricaoReceita.Text;            
            receita.categoria = cbCategoriaReceita.Text;            
            receita.valor = txtValorReceita.Text;
            receita.id_usuario = idUsuario;

            bo.Cadastrar(receita);

            atualizarGridReceitasAnoAtual();

            idReceitaSelecionada = "";
            txtDescricaoReceita.Text = "";
            cbCategoriaReceita.SelectedIndex = -1;
            txtValorReceita.Text = "";

        }
        private void btnAlterarReceita_Click(object sender, RoutedEventArgs e)
        {
            Receitas receita = new Receitas();
            ReceitasBO bo = new ReceitasBO();

            receita.id = idReceitaSelecionada;
            receita.descricao = txtDescricaoReceita.Text;
            receita.categoria = cbCategoriaReceita.Text;
            receita.valor = txtValorReceita.Text;
            receita.id_usuario = idUsuario;

            bo.Editar(receita);
                        
            atualizarGridReceitasAnoAtual();

            idReceitaSelecionada = "";
            txtDescricaoReceita.Text = "";
            cbCategoriaReceita.SelectedIndex = -1;
            txtValorReceita.Text = "";
        }
        private void btnDeletarReceita_Click(object sender, RoutedEventArgs e)
        {
            ReceitasBO receita = new ReceitasBO();
            receita.Deletar(Convert.ToInt32(idReceitaSelecionada));

            atualizarGridReceitasAnoAtual();

            idReceitaSelecionada = "";
            txtDescricaoReceita.Text = "";
            cbCategoriaReceita.SelectedIndex = -1;
            txtValorReceita.Text = "";
        }
        private void btnLimparReceita_Click(object sender, RoutedEventArgs e)
        {
            idReceitaSelecionada = "";
            txtDescricaoReceita.Text = "";
            cbCategoriaReceita.SelectedIndex = -1;
            txtValorReceita.Text = "";
        }
        #endregion

        #region Botões filtrar
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            DespesasBO bo = new DespesasBO(); 
            
            DataView resultado = bo.Filtrar(txtNomeDespesaFiltrar.Text, txtEmpresaDespesaFiltrar.Text, cbCategoriaDespesaFiltrar.Text, cbMesDespesaFiltrar.Text, cbAnoDespesaFiltrar.Text, cbEstadoDespesaFiltrar.Text, idUsuario);

            if (resultado != null) {
                dgDespesas.ItemsSource = resultado;
            } else
            {
                atualizarGridDespesasAnoAtual();
            }            
        }
        private void btnLimparFiltrar_Click(object sender, RoutedEventArgs e)
        {
            txtNomeDespesaFiltrar.Text = "";
            txtEmpresaDespesaFiltrar.Text = "";
            cbCategoriaDespesaFiltrar.SelectedIndex = -1;
            cbMesDespesaFiltrar.SelectedIndex = -1;
            cbAnoDespesaFiltrar.SelectedIndex = -1;
            cbEstadoDespesaFiltrar.SelectedIndex = -1;
        }
        #endregion

        private void btnResetLogin_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default["email"] = "";
            Settings.Default["senha"] = "";
            Settings.Default.Save();
        }
    }
}
