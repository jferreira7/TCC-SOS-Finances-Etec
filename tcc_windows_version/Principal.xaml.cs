using Microsoft.Win32;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using tcc_windows_version.Business;
using tcc_windows_version.Database;
using tcc_windows_version.Model;
using tcc_windows_version.Properties;
using tcc_windows_version.View;

namespace tcc_windows_version
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        string idDespesaSelecionada, idReceitaSelecionada, idObjetivoSelecionado;
        string nome_usuario;
        string caminho_imagem = "";        
        byte[] imagem;
        int idUsuario;
        int idRow;
        TextBox txtAnterior = null;
        
        /*
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
        */
        public MainWindow()
        {
            InitializeComponent();
            Settings.Default.Reload();

            //Para arrumar a data que aparece no DataGrid
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            //FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            //Para retirar a barra superior padrão do windows e permitir mover o programa sem ela
            /*SourceInitialized += (s, e) =>
            {
                IntPtr handle = (new WindowInteropHelper(this)).Handle;
                HwndSource.FromHwnd(handle).AddHook(new HwndSourceHook(WindowProc));
            };*/

            gdDespesas.Visibility = Visibility.Visible;
            gdFiltrar.Visibility = Visibility.Hidden;
            gdReceitas.Visibility = Visibility.Hidden;
            gdObjetivos.Visibility = Visibility.Hidden;

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

        #region Funções
        public void saldo()
        {
            ValoresBO oBO = new ValoresBO();
            tbSaldoAtual.Text = "R$" + oBO.BuscarSaldo(idUsuario);
        }
        public void reserva()
        {
            ValoresBO oBO = new ValoresBO();
            tbReservaObjetivos.Text = "R$" + oBO.BuscarReserva(idUsuario);
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

                saldo();
                reserva();                
            }
            catch
            {
                
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

                saldo();
                reserva();
            }
            catch
            {
               
            }
        }
        public void atualizarGridObjetivos()
        {
            ObjetivosBO bo = new ObjetivosBO();
            DataView resultado = bo.BuscarTodos(idUsuario);

            if (resultado != null)
            {
                dgObjetivos.ItemsSource = resultado;
                saldo();
                reserva();
            }
        }
        #endregion

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
            gdObjetivos.Visibility = Visibility.Hidden;
            gdLateralBotoes.Margin = new Thickness(-1, 125, 0, 0);

            dgReceitas.Visibility = Visibility.Hidden;
            dgDespesas.Visibility = Visibility.Visible;

            atualizarGridDespesasAnoAtual();
        }

        private void btnReceitas_Click(object sender, RoutedEventArgs e)
        {
            gdDespesas.Visibility = Visibility.Hidden;
            gdFiltrar.Visibility = Visibility.Hidden;
            gdObjetivos.Visibility = Visibility.Hidden;
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
            gdObjetivos.Visibility = Visibility.Hidden;
            gdLateralBotoes.Margin = new Thickness(-1, 255, 0, 0);

            dgReceitas.Visibility = Visibility.Hidden;
            dgDespesas.Visibility = Visibility.Visible;

            atualizarGridDespesasAnoAtual();
        }

        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            gdLateralBotoes.Margin = new Thickness(-1, 488, 0, 0);
            Configurações config = new Configurações();
            config.Show();
        }

        private void btnObjetivos_Click(object sender, RoutedEventArgs e)
        {
            gdDespesas.Visibility = Visibility.Hidden;
            gdFiltrar.Visibility = Visibility.Hidden;
            gdReceitas.Visibility = Visibility.Hidden;
            gdObjetivos.Visibility = Visibility.Hidden;
            dgReceitas.Visibility = Visibility.Hidden;
            dgDespesas.Visibility = Visibility.Hidden;

            gdLateralBotoes.Margin = new Thickness(-1, 320, 0, 0);
            gdObjetivos.Visibility = Visibility.Visible;

            atualizarGridObjetivos();
        }
        #endregion

        #region Botões borda windows
        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximizar_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
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
            dgDespesas.UnselectAll();
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
            dgReceitas.UnselectAll();
        }
        #endregion

        #region Botões filtrar
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            DespesasBO bo = new DespesasBO();

            DataView resultado = bo.Filtrar(txtNomeDespesaFiltrar.Text, txtEmpresaDespesaFiltrar.Text, cbCategoriaDespesaFiltrar.Text, cbMesDespesaFiltrar.Text, cbAnoDespesaFiltrar.Text, cbEstadoDespesaFiltrar.Text, idUsuario);

            if (resultado != null)
            {
                dgDespesas.ItemsSource = resultado;
            }
            else
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
            dgDespesas.UnselectAll();
        }
        #endregion

        #region Botões objetivos
        private void btnAdicionarObjetivo_Click(object sender, RoutedEventArgs e)
        {
            Objetivos objetivo = new Objetivos();
            objetivo.nome = txtNomeObjetivo.Text;
            objetivo.preco = txtPrecoObjetivo.Text;            
            objetivo.id_usuario = idUsuario;

            if(txtValorInicialObjetivo.Text != "")
            {
                objetivo.valor_guardado = txtValorInicialObjetivo.Text;
            }
            else
            {
                objetivo.valor_guardado = "0";
            }            
            
            if(caminho_imagem == "" || caminho_imagem == null)
            {
                imagem = System.Convert.FromBase64String(Settings.Default["imgObjetivo"].ToString());
            }

            objetivo.imagem_bytes = imagem;

            if (Convert.ToDecimal(objetivo.valor_guardado) > Convert.ToDecimal(objetivo.preco))
            {
                MessageBox.Show("O preço está menor do que está sendo guardado.");
            }
            else
            {
                ObjetivosBO oBO = new ObjetivosBO();
                oBO.Cadastrar(objetivo);
            }            

            atualizarGridObjetivos();
            limparObjetivo();
            caminho_imagem = "";
        }
        private void btnImageObjetivo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image files (*.jpg, *.png)|*.jpg; *.png";
            openFileDialog1.Multiselect = false;
            if (openFileDialog1.ShowDialog() == true)
            {
                caminho_imagem = openFileDialog1.FileName;

                if (caminho_imagem != "")
                {
                    btnImageObjetivo.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(caminho_imagem, UriKind.Relative)) };
                    string FileName = caminho_imagem;

                    FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imagem = br.ReadBytes((int)fs.Length);

                    /*using (var tw = new StreamWriter(@"D:\Example.txt", true))
                    {
                        tw.WriteLine(System.Convert.ToBase64String(imagem));
                    }*/  
                    
                    br.Close();
                    fs.Close();
                }                
            }
        }
        private void btnLimparObjetivo_Click(object sender, RoutedEventArgs e)
        {
            limparObjetivo();
            dgObjetivos.UnselectAll();
        }
        private void btnDeletarObjetivo_Click(object sender, RoutedEventArgs e)
        {
            ObjetivosBO objetivo = new ObjetivosBO();
            objetivo.Deletar(Convert.ToInt32(idObjetivoSelecionado));

            atualizarGridObjetivos();
            limparObjetivo();
        }
        private void btnAlterarObjetivo_Click(object sender, RoutedEventArgs e)
        {
            Objetivos objetivo = new Objetivos();
            ObjetivosBO oBO = new ObjetivosBO();

            objetivo.id = Convert.ToInt32(idObjetivoSelecionado);
            objetivo.nome = txtNomeObjetivo.Text;
            objetivo.preco = txtPrecoObjetivo.Text;
            objetivo.imagem_bytes = imagem;
            objetivo.id_usuario = idUsuario;

            
            DataView dv = oBO.SelecionarUm(objetivo.id);
            DataTable dt = dv.ToTable();

            if (dt != null)
            {
                objetivo.valor_guardado = dt.Rows[0]["valor_guardado"].ToString();
                objetivo.estado = dt.Rows[0]["estado"].ToString();
            }
           
            oBO.Editar(objetivo);

            atualizarGridObjetivos();
            limparObjetivo();
        }
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        public void limparObjetivo()
        {
            idDespesaSelecionada = "";
            txtNomeObjetivo.Text = "";
            txtPrecoObjetivo.Text = "";
            btnImageObjetivo.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/imageIcon.png", UriKind.RelativeOrAbsolute)) };
            txtValorInicialObjetivo.Text = "";
            txtValorInicialObjetivo.IsEnabled = true;
            gdDetalhes.Visibility = Visibility.Hidden;
        }
        #endregion

        DataRowView row_selected;
        private void btnDetalhesObjetivo_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            row_selected = button.DataContext as DataRowView;
            if (row_selected != null)
            {
                gdDetalhes.Visibility = Visibility.Visible;
                tbPorcentagemObjetivo.Text = row_selected["porcentagem"].ToString() + "%";
                tbValorGuardadoObjetivo.Text = row_selected["valor_guardado"].ToString();
                tbValorRestanteObjetivo.Text = row_selected["valor_restante"].ToString();
                tbTempoTotalObjetivo.Text = "0";
                tbDataInsercaoObjetivo.Text = row_selected["data_insercao"].ToString().Substring(0, 10);                
                if (row_selected["data_finalizacao"].ToString() != "")
                {
                    tbDataFinalizacaoObjetivo.Text = row_selected["data_finalizacao"].ToString().Substring(0, 10);
                    TimeSpan durantion = Convert.ToDateTime(row_selected["data_finalizacao"]) - Convert.ToDateTime(row_selected["data_insercao"]);
                    tbTempoTotalObjetivo.Text = Convert.ToInt32(durantion.TotalDays).ToString() + " dias";
                }
                else
                {
                    tbDataFinalizacaoObjetivo.Text = "";
                    TimeSpan durantion = DateTime.Now - Convert.ToDateTime(row_selected["data_insercao"]);
                    tbTempoTotalObjetivo.Text = Convert.ToInt32(durantion.TotalDays).ToString() + " dias";
                }

                if (row_selected["estado"].ToString() == "Finalizado")
                {
                    btnCompradoObjetivo.Visibility = Visibility.Visible;
                }
                else
                {
                    btnCompradoObjetivo.Visibility = Visibility.Hidden;
                }
            }
        }
        
        private void btnAdicionarValorObjetivo_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataRowView row_selected = button.DataContext as DataRowView;

            if (idRow == Convert.ToInt32(row_selected["id"]))
            {
                if(row_selected["valor_guardado"].ToString() != row_selected["preco"].ToString())
                {
                    double novoValorGuardado = Convert.ToDouble(row_selected["valor_guardado"]) + Convert.ToDouble(txtAnterior.Text);
                    double preco = Convert.ToDouble(row_selected["preco"]);
                    ObjetivosBO oBO = new ObjetivosBO();

                    if (novoValorGuardado <= preco)
                    {
                        oBO.AtualizarValorGuardado(idRow, idUsuario, novoValorGuardado);
                    }
                    else
                    {
                        novoValorGuardado = preco;
                        oBO.AtualizarValorGuardado(idRow, idUsuario, novoValorGuardado);
                    }
                    atualizarGridObjetivos();
                }
                else
                {
                    MessageBox.Show("O objetivo já foi completo!");
                }              
            }
            else
            {
                MessageBox.Show("Insira o valor na linha correta.");                
            }
        }
        private void btnRemoverValorObjetivo_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DataRowView row_selected = button.DataContext as DataRowView;

            if (idRow == Convert.ToInt32(row_selected["id"]) && Convert.ToDouble(txtAnterior.Text) != 0)
            {
                double novoValorGuardado = Convert.ToDouble(row_selected["valor_guardado"]) - Convert.ToDouble(txtAnterior.Text);
                double preco = Convert.ToDouble(row_selected["preco"]);
                ObjetivosBO oBO = new ObjetivosBO();

                if (novoValorGuardado >= 0)
                {
                    oBO.AtualizarValorGuardado(idRow, idUsuario, novoValorGuardado);
                }
                else
                {
                    novoValorGuardado = 0;
                    oBO.AtualizarValorGuardado(idRow, idUsuario, novoValorGuardado);
                }

                atualizarGridObjetivos();
            }
            else
            {
                MessageBox.Show("Insira o valor na linha correta.");
            }
        }

        

        public void txtValorObjetivoAddRemove_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            DataRowView row_selected = txt.DataContext as DataRowView;

            txtAnterior = txt;

            if (row_selected != null)
            {
                idRow = Convert.ToInt32(row_selected["id"]);                
            }
        }

        

        private void txtValorObjetivoAddRemove_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (txtAnterior != null)
            {
                txtAnterior.Text = "";
                txtAnterior = null;
            }
        }   
        public void dgObjetivos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gdDetalhes.Visibility = Visibility.Hidden;
            DataGrid dg = (DataGrid)sender;            
            DataRowView row_selected = dg.SelectedItem as DataRowView;                       

            if (row_selected != null)
            {
                idObjetivoSelecionado = row_selected["id"].ToString();
                txtNomeObjetivo.Text = row_selected["nome"].ToString();
                txtPrecoObjetivo.Text = row_selected["preco"].ToString();
                txtValorInicialObjetivo.IsEnabled = false;

                imagem = (byte[])row_selected["imagem"];
                btnImageObjetivo.Background = new ImageBrush { ImageSource = ToImage(imagem) };                
            }
        }

        private void btnCompradoObjetivo_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(row_selected["id"]);
            
            ObjetivosBO oBO = new ObjetivosBO();
            oBO.AtualizarEstado(id, idUsuario, "Comprado");

            atualizarGridObjetivos();
        }
    }
}
