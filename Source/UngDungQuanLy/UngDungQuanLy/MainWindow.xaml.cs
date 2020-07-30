using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;

namespace UngDungQuanLy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string excecutable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(excecutable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
        }


        bool loadTabHangHoa = false;
        bool loadTabGiaoDich = false;
        bool loadTabThongKe = false;



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// HÀM XỬ LÝ CHO Button "THOÁT"

        /// <summary>
        /// Hàm để thoát phần mềm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MBRs = MessageBox.Show("Bạn có muốn thoát", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MBRs == MessageBoxResult.Yes)
            {
                this.Close();
            }
            else
            {
                return;
            }
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// CÁC HÀM XỬ LÝ CHO TAB "HÀNG HÓA"



        /// <summary>
        /// Reload dữ liệu cho tab Hàng hóa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabHangHoa_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (loadTabHangHoa == false)
            {
                loadTabHangHoa = true;
                loadTabGiaoDich = false;
                loadTabThongKe = false;

                TabHangHoa.IsSelected = true;

                removeAllTabHangHoa();
                ShowTabHangHoa();

                lockcontrols();
                BtnThem.IsEnabled = false;
                BtnSua.IsEnabled = false;
                BtnXoa.IsEnabled = false;
            }
        }


        int pageNumber = 1;
        int recordNumber = 28;


        /// <summary>
        /// Hàm dùng để khóa các control
        /// </summary>
        void lockcontrols()
        {
            TbTenHangHoa.IsEnabled = false;
            CbLoai.IsEnabled = false;
            TbThemChungLoai.IsEnabled = false;
            TbSoLuong.IsEnabled = false;
            TbGiaNiemyet.IsEnabled = false;
            TbGiaBanLe.IsEnabled = false;
            TbGiaThucTe.IsEnabled = false;
            DPNgayThem.IsEnabled = false;

            BtnChonHinh.IsEnabled = false;
            BtnThemChungLoai.IsEnabled = false;
        }


        /// <summary>
        /// Hàm dùng để mở các control
        /// </summary>
        void unlockcontrols()
        {
            TbTenHangHoa.IsEnabled = true;
            CbLoai.IsEnabled = true;
            TbThemChungLoai.IsEnabled = true;
            TbSoLuong.IsEnabled = true;
            TbGiaNiemyet.IsEnabled = true;
            TbGiaBanLe.IsEnabled = true;
            TbGiaThucTe.IsEnabled = true;
            DPNgayThem.IsEnabled = true;

            BtnChonHinh.IsEnabled = true;
            BtnThemChungLoai.IsEnabled = true;
        }


        void removeAllTabHangHoa()
        {
            TbTenHangHoa.Text = "";
            CbLoai.Text = "";
            TbThemChungLoai.Text = "";
            TbSoLuong.Text = "";
            TbGiaNiemyet.Text = "";
            TbGiaBanLe.Text = "";
            TbGiaThucTe.Text = "";
            DPNgayThem.Text = "";
            ImageProduct.Source = null;
        }

        private List<HangHoa> LoadRecord(int page,int recordNum)
        {
            List<HangHoa> result = new List<HangHoa>();
            using (QuanLyCuaHangEntities db = new QuanLyCuaHangEntities())
            {              
                result = db.HangHoa.OrderBy(i=>i.Id). Skip((page - 1) * recordNum).Take(recordNum).ToList();
            }
            return result;
        }


        void ShowTabHangHoa()
        {
            var db = new QuanLyCuaHangEntities();

            //Thêm dữ liệu từ sql (Table ChungLoai) vào combobox "Loai"
            CbLoai.ItemsSource = db.Procedure_LayTatCaTenLoai().ToList();

            //Thêm dữ liệu vào 2 combobox "CbLoaiTimKiem" và "CbLoaiSapXep"
            string[] ListTimKiem = { "Tìm kiếm theo", "All", "Mã hàng hóa", "Loại", "Tên", "Số lượng",
                          "Giá niêm yết", "Giá bán lẻ", "Giá thực tế", "Ngày cập nhật" };
            string[] ListSapXep = { "Sắp xếp theo", "Mới nhất", "Số lượng tăng", "Số lượng giảm",
                                  "Giá tăng dần", "Giá giảm dần", "Theo tên A->Z", "Theo tên Z->A",
                                   "Theo loại A->Z", "Theo loại Z->A" };

            CbLoaiTimKiem.ItemsSource = ListTimKiem.ToList();
            CbLoaiSapXep.ItemsSource = ListSapXep.ToList();

            CbLoaiTimKiem.SelectedIndex = 0;
            CbLoaiSapXep.SelectedIndex = 0;

            //Hiển thị danh sách hàng hóa
            dataGrid.ItemsSource = LoadRecord(pageNumber, recordNumber);
        }       


        /// <summary>
        /// Load dữ liệu lên tab Hàng hóa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabHangHoa_Loaded(object sender, RoutedEventArgs e)
        {            
            lockcontrols();
            BtnThem.IsEnabled = false;
            BtnSua.IsEnabled = false;
            BtnXoa.IsEnabled = false;
            
            ShowTabHangHoa();
        }


        string optionUnlockButton = "";
        private void BtnThemHangHoa_Click(object sender, RoutedEventArgs e)
        {
            BtnThem.IsEnabled = true;
            BtnSua.IsEnabled = false;
            BtnXoa.IsEnabled = false;

            unlockcontrols();

            optionUnlockButton = "add";
        }

        private void BtnSuaHangHoa_Click(object sender, RoutedEventArgs e)
        {
            BtnThem.IsEnabled = false;
            BtnSua.IsEnabled = true;
            BtnXoa.IsEnabled = false;

            unlockcontrols();

            optionUnlockButton = "edit";
        }

        private void BtnXoaHangHoa_Click(object sender, RoutedEventArgs e)
        {
            BtnThem.IsEnabled = false;
            BtnSua.IsEnabled = false;
            BtnXoa.IsEnabled = true;           

            unlockcontrols();

            TbThemChungLoai.IsEnabled = false;
            BtnThemChungLoai.IsEnabled = false;

            optionUnlockButton = "delete";
        }


        String pathImage = "";
        /// <summary>
        /// Hàm để chọn hình từ thư mục
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChonHinh_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "png files(*.png)|*.png|jpg files (*.jpg)|*.jpg|All files(*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ImageProduct.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                pathImage = openFileDialog.FileName;
            }
        }


        /// <summary>
        /// Hàm dùng để thêm 1 loại hàng hóa mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnThemChungLoai_Click(object sender, RoutedEventArgs e)
        {
            if (TbThemChungLoai.Text != "")
            {
                var db = new QuanLyCuaHangEntities();
                LoaiHangHoa chungloai = new LoaiHangHoa { TenLoai = TbThemChungLoai.Text };
                db.LoaiHangHoa.Add(chungloai);
                db.SaveChanges();
                CbLoai.ItemsSource = db.Procedure_LayTatCaTenLoai().ToList();

                MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show("Bạn cần điền tên chủng loại cần thêm!!!");
            }
        }


        int id;//Id của hàng hóa đang đươc chọn
        /// <summary>
        /// Hàm xử lý khi chọn vào 1 dòng trong DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            unlockcontrols();
            BtnThem.IsEnabled = false;
            if (optionUnlockButton=="edit")
            {               
                BtnSua.IsEnabled = true;
                BtnXoa.IsEnabled = false;
            }
            if (optionUnlockButton == "delete")
            {
                BtnSua.IsEnabled = false;
                BtnXoa.IsEnabled = true;

                TbThemChungLoai.IsEnabled = false;
                BtnThemChungLoai.IsEnabled = false;
            }

            HangHoa hh = dataGrid.SelectedItem as HangHoa;
            if(hh != null)
            {
                id = hh.Id;
                TbTenHangHoa.Text = hh.Ten;
                CbLoai.Text = hh.Loai;
                TbSoLuong.Text = hh.SoLuong.ToString();
                TbGiaNiemyet.Text = hh.GiaNiemYet.ToString();
                TbGiaBanLe.Text = hh.GiaBanLe.ToString();
                TbGiaThucTe.Text = hh.GiaThucTe.ToString();
                DPNgayThem.SelectedDate = hh.NgayCapNhat;

                if (hh.HinhAnh != null)
                {
                    try
                    {
                        byte[] imgBytes = (byte[])hh.HinhAnh;
                        MemoryStream ms = new MemoryStream(imgBytes, 0, imgBytes.Length);
                        ms.Write(imgBytes, 0, imgBytes.Length);

                        var imageSource = new BitmapImage();
                        ms.Position = 0;
                        imageSource.BeginInit();
                        imageSource.StreamSource = ms;
                        imageSource.EndInit();

                        ImageProduct.Source = imageSource;
                    }
                    catch
                    {
                        ImageProduct.Source = null;
                    }
                    
                }
                else
                {
                    ImageProduct.Source = null;
                }
            }
        }


        /// <summary>
        /// Hàm dùng để thêm 1 hàng hóa mới khi bấm vào button "THÊM"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnThem_Click(object sender, RoutedEventArgs e)
        {
            unlockcontrols();
            if (TbTenHangHoa.Text != "" && CbLoai.SelectedItem.ToString() != "" && TbSoLuong.Text != ""
                && TbGiaBanLe.Text != "" && TbGiaNiemyet.Text != "" && DPNgayThem.Text != "")
            {
                try
                {
                    var hangHoa = new HangHoa();
                    hangHoa.Loai = CbLoai.SelectedItem.ToString();
                    hangHoa.Ten = TbTenHangHoa.Text;
                    hangHoa.SoLuong = int.Parse(TbSoLuong.Text);
                    hangHoa.GiaNiemYet = int.Parse(TbGiaNiemyet.Text);
                    hangHoa.GiaBanLe = int.Parse(TbGiaBanLe.Text);
                    hangHoa.NgayCapNhat = DPNgayThem.SelectedDate;

                    if (pathImage != "")
                    {
                        //Chuyển image thành dãy bit
                        FileStream fs;
                        fs = new FileStream(pathImage, FileMode.Open, FileAccess.Read);
                        byte[] picbyte = new byte[fs.Length];
                        fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
                        fs.Close();

                        hangHoa.HinhAnh = picbyte;

                        pathImage = "";
                    }

                    var db = new QuanLyCuaHangEntities();
                    db.HangHoa.Add(hangHoa);
                    db.SaveChanges();

                    MessageBox.Show("Thêm thành công");
                    TBlThongBao.Text = "";

                    dataGrid.ItemsSource = db.HangHoa.ToList();
                }
                catch
                {
                    TBlThongBao.Text = "Vui lòng nhập đúng định dạng dữ liệu!!!";
                }
                
            }
            else
            {
                TBlThongBao.Text = "Vui lòng điền đầy đủ thông tin trước khi thêm!!!";
            }
        }

        

        private void BtnSua_Click(object sender, RoutedEventArgs e)
        {
            unlockcontrols();

            if (TbTenHangHoa.Text != "" && CbLoai.SelectedItem.ToString() != "" && TbSoLuong.Text != ""
                && TbGiaBanLe.Text != "" && TbGiaNiemyet.Text != "" && DPNgayThem.Text != "")
            {
                try
                {
                    var db = new QuanLyCuaHangEntities();

                    HangHoa hh = dataGrid.SelectedItem as HangHoa;//chọn 1 dòng từ DataGrid
                    id = hh.Id;
                    var hh1 = db.HangHoa.Find(id);

                    hh1.Loai = CbLoai.SelectedItem.ToString();
                    hh1.Ten = TbTenHangHoa.Text;
                    hh1.SoLuong = int.Parse(TbSoLuong.Text);
                    hh1.GiaNiemYet = int.Parse(TbGiaNiemyet.Text);
                    hh1.GiaBanLe = int.Parse(TbGiaBanLe.Text);
                    hh1.NgayCapNhat = DPNgayThem.SelectedDate;
                    if (pathImage != "")
                    {
                        //Chuyển image thành dãy bit
                        FileStream fs;
                        fs = new FileStream(pathImage, FileMode.Open, FileAccess.Read);
                        byte[] picbyte = new byte[fs.Length];
                        fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
                        fs.Close();

                        hh1.HinhAnh = picbyte;

                        pathImage = "";
                    }

                    MessageBoxResult MBRs = MessageBox.Show("Bạn chắc chắn muốn sửa", "Xác nhận",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (MBRs == MessageBoxResult.Yes)
                    {
                        db.SaveChanges();

                        TBlThongBao.Text = "";                        

                        dataGrid.ItemsSource = db.HangHoa.ToList();

                        MessageBox.Show("Sửa thành công");
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    TBlThongBao.Text = "Vui lòng nhập đúng định dạng dữ liệu!!!";
                }

            }
            else
            {
                TBlThongBao.Text = "Vui lòng điền đầy đủ thông tin trước khi sửa!!!";
            }
        }


        private void BtnXoa_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MBRs = MessageBox.Show("Bạn chắc chắn muốn xóa hàng hóa này?", "Xác nhận",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MBRs == MessageBoxResult.Yes)
            {
                TbTenHangHoa.Text = "";
                CbLoai.Text = "";
                TbSoLuong.Text = "";
                TbGiaNiemyet.Text = "";
                TbGiaBanLe.Text = "";
                TbGiaThucTe.Text = "";
                DPNgayThem.SelectedDate = null;
                ImageProduct.Source = null;

                HangHoa hh = dataGrid.SelectedItem as HangHoa;
                var db = new QuanLyCuaHangEntities();
                db.Procedure_Xoa1HangHoa(id);                

                dataGrid.ItemsSource = db.HangHoa.ToList();

                MessageBox.Show("Xóa thành công");
            }
            else
            {
                return;
            }
        }


        /// <summary>
        /// Hàm thực thi việc sắp xếp các hàng hóa theo một thứ tự nào đó
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbLoaiSapXep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var db = new QuanLyCuaHangEntities();
            if (CbLoaiSapXep.SelectedItem.ToString() == "Sắp xếp theo")
            {
                dataGrid.ItemsSource = db.HangHoa.ToList();
            }
            else if (CbLoaiSapXep.SelectedItem.ToString() == "Mới nhất")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    SqlQuery("SELECT * FROM HangHoa  ORDER BY NgayCapNhat DESC").ToList();
            }
            else if (CbLoaiSapXep.SelectedItem.ToString() == "Số lượng tăng")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    SqlQuery("SELECT * FROM HangHoa  ORDER BY SoLuong ASC").ToList();
            }
            else if (CbLoaiSapXep.SelectedItem.ToString() == "Số lượng giảm")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    SqlQuery("SELECT * FROM HangHoa  ORDER BY SoLuong DESC").ToList();
            }
            else if (CbLoaiSapXep.SelectedItem.ToString() == "Giá tăng dần")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    SqlQuery("SELECT * FROM HangHoa  ORDER BY GiaNiemYet ASC").ToList();
            }
            else if (CbLoaiSapXep.SelectedItem.ToString() == "Giá giảm dần")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    SqlQuery("SELECT * FROM HangHoa  ORDER BY GiaNiemYet DESC").ToList();
            }
            else if (CbLoaiSapXep.SelectedItem.ToString() == "Theo tên A->Z")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    SqlQuery("SELECT * FROM HangHoa  ORDER BY CONVERT(nvarchar, Ten) ASC").ToList();
            }
            else if (CbLoaiSapXep.SelectedItem.ToString() == "Theo tên Z->A")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    SqlQuery("SELECT * FROM HangHoa  ORDER BY CONVERT(nvarchar, Ten) DESC").ToList();
            }
            else if (CbLoaiSapXep.SelectedItem.ToString() == "Theo loại A->Z")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    SqlQuery("SELECT * FROM HangHoa  ORDER BY CONVERT(nvarchar, Loai) ASC").ToList();
            }
            else if (CbLoaiSapXep.SelectedItem.ToString() == "Theo loại Z->A")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    SqlQuery("SELECT * FROM HangHoa  ORDER BY CONVERT(nvarchar, Loai) DESC").ToList();
            }
        }


        /// <summary>
        /// Hàm thực thi việc tìm kiếm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            var db = new QuanLyCuaHangEntities();
            if (CbLoaiTimKiem.SelectedItem.ToString() == "Mã hàng hóa")
            {
                dataGrid.ItemsSource = db.HangHoa.Where(hh => hh.Id.ToString().Contains(TbTimKiem.Text)).ToList();
            }
            else if (CbLoaiTimKiem.SelectedItem.ToString()=="Loại")
            {
                dataGrid.ItemsSource = db.HangHoa.Where(hh => hh.Loai.Contains(TbTimKiem.Text)).ToList();
            }
            else if (CbLoaiTimKiem.SelectedItem.ToString()=="Tên")
            {
                dataGrid.ItemsSource = db.HangHoa.Where(hh => hh.Ten.Contains(TbTimKiem.Text)).ToList();
            }
            else if (CbLoaiTimKiem.SelectedItem.ToString() == "Số lượng")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    Where(hh => hh.SoLuong.ToString().Contains(TbTimKiem.Text)).ToList();
            }
            else if (CbLoaiTimKiem.SelectedItem.ToString() == "Giá niêm yết")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    Where(hh => hh.GiaNiemYet.ToString().Contains(TbTimKiem.Text)).ToList();
            }
            else if (CbLoaiTimKiem.SelectedItem.ToString() == "Giá bán lẻ")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    Where(hh => hh.GiaBanLe.ToString().Contains(TbTimKiem.Text)).ToList();
            }
            else if (CbLoaiTimKiem.SelectedItem.ToString() == "Giá thực tế")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    Where(hh => hh.GiaThucTe.ToString().Contains(TbTimKiem.Text)).ToList();
            }
            else if (CbLoaiTimKiem.SelectedItem.ToString() == "Ngày cập nhật")
            {
                dataGrid.ItemsSource = db.HangHoa.
                    Where(hh => hh.NgayCapNhat.ToString().Contains(TbTimKiem.Text)).ToList();
            }
            else
            {
                dataGrid.ItemsSource = db.HangHoa.SqlQuery("SELECT * FROM HangHoa").ToList();
            }
        }


        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTrangTruoc_Click(object sender, RoutedEventArgs e)
        {
            if(pageNumber-1>0)
            {
                pageNumber--;
                TBlTrangHienTai.Text = pageNumber.ToString();
                dataGrid.ItemsSource = LoadRecord(pageNumber, recordNumber);
            }
        }


        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTrangSau_Click(object sender, RoutedEventArgs e)
        {
            int totalRecord = 0;
            var db = new QuanLyCuaHangEntities();
            totalRecord = db.HangHoa.Count();
            if(pageNumber - 1 < totalRecord / recordNumber)
            {
                pageNumber++;
                TBlTrangHienTai.Text = pageNumber.ToString();
                dataGrid.ItemsSource = LoadRecord(pageNumber, recordNumber);
            }
            
        }



        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// CÁC HÀM XỬ LÝ CHO TAB "GIAO DỊCH"


        private void TabGiaoDich_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (loadTabGiaoDich == false)
            {
                TabGiaoDich.IsSelected = true;

                loadTabHangHoa = false;
                loadTabGiaoDich = true;
                loadTabThongKe = false;

                var db = new QuanLyCuaHangEntities();
                dataGrid1.ItemsSource = db.HangHoa.ToList();
            }
        }


        void ShowTabGiaoDich()
        {
            string[] ListTimKiem = { "Tìm kiếm theo", "All", "Loại", "Tên", "Số lượng", "Giá bán lẻ" };
            CbLoaiTimKiem1.ItemsSource = ListTimKiem.ToList();
            CbLoaiTimKiem1.SelectedIndex = 0;
        }


        private void TabGiaoDich_Loaded(object sender, RoutedEventArgs e)
        {
            ShowTabGiaoDich();
            var db = new QuanLyCuaHangEntities();
            dataGrid1.ItemsSource = db.HangHoa.ToList();
        }


        private void BtnTimKiem1_Click(object sender, RoutedEventArgs e)
        {
            var db = new QuanLyCuaHangEntities();

            if (CbLoaiTimKiem1.SelectedItem.ToString() == "Loại")
            {
                dataGrid1.ItemsSource = db.HangHoa.Where(hh => hh.Loai.Contains(TbTimKiem1.Text)).ToList();
            }
            else if (CbLoaiTimKiem1.SelectedItem.ToString() == "Tên")
            {
                dataGrid1.ItemsSource = db.HangHoa.Where(hh => hh.Ten.Contains(TbTimKiem1.Text)).ToList();
            }
            else if (CbLoaiTimKiem1.SelectedItem.ToString() == "Số lượng")
            {
                dataGrid1.ItemsSource = db.HangHoa.
                    Where(hh => hh.SoLuong.ToString().Contains(TbTimKiem1.Text)).ToList();
            }
            else if (CbLoaiTimKiem1.SelectedItem.ToString() == "Giá bán lẻ")
            {
                dataGrid1.ItemsSource = db.HangHoa.
                    Where(hh => hh.GiaBanLe.ToString().Contains(TbTimKiem1.Text)).ToList();
            }
            else
            {
                dataGrid1.ItemsSource = db.HangHoa.SqlQuery("SELECT * FROM HangHoa").ToList();
            }
        }



        private void BtnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MBRs = MessageBox.Show("Bạn có muốn tạo đơn hàng mới", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MBRs == MessageBoxResult.Yes)
            {
                ShowTabGiaoDich();
                var db = new QuanLyCuaHangEntities();
                dataGrid1.ItemsSource = db.HangHoa.ToList();
                dataGrid2.ItemsSource = null;

                TbSoHoaDon.Text = "";
                DPNgayGiaoDich.Text = "";
                TbKhachHang.Text = "";
                TbDiaChi.Text = "";
                TbSdt.Text = "";

                TblTongTien.Text = "";
                TbGiam.Text = "";
                TblTongCong.Text = "";
                TbTienKhachDua.Text = "";
                TblTienTraLai.Text = "";

                TbSoLuongTab2.Text = "";
                TbGiamGiaTab2.Text = "";

                TblDaGiaoHang.Background = Brushes.Red;

                TongTien = 0;
                STT = 0;
                int n = datas.Count();
                for (int i = n - 1; i >= 0; i--)
                {
                    datas.Remove(datas[i]);
                }
            }
            else
            {
                return;
            }
        }


        class Data
        {
            public int id { get; set; }
            public int stt { get; set; }
            public String ten { get; set; }
            public int soLuong { get; set; }
            public int donGia { get; set; }
            public int giam { get; set; }
            public int thanhTien { get; set; }

        }


        int TongTien = 0;
        int STT = 0;
        List<Data> datas = new List<Data>();


        /// <summary>
        /// Chọn 1 hàng hóa trong kho vào danh sách hàng hóa mà khách hàng định mua
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChon_Click(object sender, RoutedEventArgs e)
        {
            var db = new QuanLyCuaHangEntities();

            HangHoa hh = dataGrid1.SelectedItem as HangHoa;//chọn 1 dòng từ DataGrid2
                       
            if(hh!=null)
            {
                if (hh.SoLuong <= 0)
                {
                    MessageBox.Show("Đã hết hàng!!!");
                }
                else
                {
                    STT++;
                    int SOLUONG = 1;
                    int THANHTIEN = int.Parse(hh.GiaBanLe.ToString()) * SOLUONG;
                    datas.Add(new Data()
                    {
                        id = hh.Id,
                        stt = STT,
                        ten = hh.Ten,
                        soLuong = SOLUONG,
                        donGia = int.Parse(hh.GiaBanLe.ToString()),
                        giam = 0,
                        thanhTien = THANHTIEN
                    });

                    TongTien += THANHTIEN;
                    TblTongTien.Text = TongTien.ToString();

                    dataGrid2.ItemsSource = null;
                    dataGrid2.ItemsSource = datas;
                }
            }
            
        }


        /// <summary>
        /// Bỏ chọn 1 hàng hóa trong danh sách hàng hóa khách hàng định mua
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBoChon_Click(object sender, RoutedEventArgs e)
        {
            var hh = dataGrid2.SelectedItem as Data;
            int soThuTu = hh.stt;
            for(int i = soThuTu - 1; i < datas.Count()-1; i++)
            {
                datas[i].ten = datas[i+1].ten;
                datas[i].soLuong = datas[i+1].soLuong;
                datas[i].donGia = datas[i+1].donGia;
                datas[i].giam = datas[i+1].giam;
                datas[i].thanhTien = datas[i+1].thanhTien;
            }
            datas.Remove(datas[datas.Count() - 1]);

            STT = datas.Count();

            dataGrid2.ItemsSource = null;
            dataGrid2.ItemsSource = datas;
        }


        /// <summary>
        /// Chọn 1 dòng trong dataGrid2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dt = dataGrid2.SelectedItem as Data;
            if (dt != null)
            {
                TbSoLuongTab2.Text = dt.soLuong.ToString();
                TbGiamGiaTab2.Text = dt.giam.ToString();
            }
        }


        /// <summary>
        /// Hàm dùng để xác nhận chọn số lượng và phần trăm giảm giá cho 1 hàng hóa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnXacNhan_Click(object sender, RoutedEventArgs e)
        {
            var dt = dataGrid2.SelectedItem as Data;
            if (dt != null)
            {
                int ma = dt.stt - 1;

                var db = new QuanLyCuaHangEntities();
                var hh1 = db.HangHoa.Find(datas[ma].id);
                int soLuong = int.Parse(TbSoLuongTab2.Text);
                if (hh1.SoLuong > 0)
                {
                    if (soLuong <= hh1.SoLuong && soLuong > 0)
                    {
                        int giam = int.Parse(TbGiamGiaTab2.Text);
                        datas[ma].soLuong = soLuong;
                        datas[ma].giam = giam;
                        datas[ma].thanhTien = (datas[ma].donGia * (100 - giam) / 100) * soLuong;

                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = datas;

                        int tongtien = 0;
                        for (int i = 0; i < datas.Count(); i++)
                        {
                            tongtien += datas[i].thanhTien;
                        }
                        TblTongTien.Text = tongtien.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Số lượng hàng hóa trong kho không đủ!!! Vui lòng chọn lại số lượng!!!");
                    }
                }
                else
                {
                    MessageBox.Show("Đã hết hàng");
                }

            }
        }


        /// <summary>
        /// Hàm dùng để tính tổng cộng tiền mà khách hàng phải trả sau khi đã có giảm giá
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTinhTien1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tongCong = int.Parse(TblTongTien.Text);
                int giam = int.Parse(TbGiam.Text);
                if (giam >= 0 && giam <= 100)
                {
                    tongCong -= tongCong * giam / 100;

                    TblTongCong.Text = tongCong.ToString();
                }
                else
                {
                    MessageBox.Show("Vui lòng điền lại thông tin!!!");
                }

            }
            catch
            {
                MessageBox.Show("Vui lòng điền lại thông tin!!!");
            }
        }


        /// <summary>
        /// Hàm dùng để tính số tiền phải trả lại cho khách hàng
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTinhTien2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tongCong = int.Parse(TblTongCong.Text);
                int traLai = int.Parse(TbTienKhachDua.Text) - tongCong;

                TblTongCong.Text = tongCong.ToString();
                TblTienTraLai.Text = traLai.ToString();
            }
            catch
            {
                MessageBox.Show("Vui lòng điền lại thông tin!!!");
            }

        }


        /// <summary>
        /// Hàm xử lý khi click vào button "Giao hàng-thanh toán"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            if (datas.Count <= 0)
            {
                MessageBox.Show("Vui lòng chọn các hàng hóa muốn bán!!!");
            }
            else
            {
                if (DPNgayGiaoDich.Text == "")
                {
                    MessageBox.Show("Vui lòng chọn ngày giao dịch!!!");
                }
                else
                {
                    int check = 0;
                    var db = new QuanLyCuaHangEntities();

                    for (int i = 0; i < datas.Count(); i++)
                    {
                        try
                        {
                            var gd = new GiaoDich();
                            int shd = 0;
                            if (db.GiaoDich.Count() != 0)
                            {
                                shd = db.Database.SqlQuery<int>("SELECT MAX(SoHoaDon) FROM GiaoDich").FirstOrDefault<int>() + 1;
                            }
                            gd.SoHoaDon = shd;
                            gd.NgayGiaoDich = DPNgayGiaoDich.SelectedDate;
                            gd.TenKhachHang = TbKhachHang.Text;
                            gd.DiaChi = TbDiaChi.Text;
                            gd.Sdt = TbSdt.Text;

                            gd.MaHangHoa = datas[i].id;
                            gd.SoLuong = datas[i].soLuong;
                            gd.DonGia = datas[i].donGia;
                            gd.Giam = datas[i].giam;
                            gd.ThanhTien = datas[i].thanhTien;
                            gd.TenHangHoa = datas[i].ten;

                            db.GiaoDich.Add(gd);

                            //Thay đổi số lượng hàng hóa trong kho
                            var hh = db.HangHoa.Find(datas[i].id);
                            hh.SoLuong -= datas[i].soLuong;

                            check++;

                        }
                        catch
                        {
                            return;
                        }
                    }
                    if (check == datas.Count())
                    {
                        db.SaveChanges();
                        dataGrid1.ItemsSource = db.HangHoa.ToList();
                        MessageBox.Show("Thanh toán thành công");
                        TblDaGiaoHang.Background = Brushes.Green;
                    }
                }
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// CÁC HÀM XỬ LÝ CHO TAB "THỐNG KÊ"


        private void TabThongKe_Loaded(object sender, RoutedEventArgs e)
        {
            string[] ListSapXep = { "Sắp xếp theo", "Ngày giảm dần",
                                  "Ngày tăng dần" };
            CbSapXepTab3.ItemsSource = ListSapXep.ToList();
            CbSapXepTab3.SelectedIndex = 0;
        }


        private void TabThongKe_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (loadTabThongKe == false)
            {
                TabThongKe.IsSelected = true;

                loadTabHangHoa = false;
                loadTabGiaoDich = false;
                loadTabThongKe = true;

                dataGrid3.ItemsSource = null;
                dataGrid4.ItemsSource = null;
                TblKhoangThoiGian.Text = "";
                DPNgayBatDau.Text = "";
                DPNgayKetThuc.Text = "";
                CbSapXepTab3.SelectedItem = 0;
            }
        }


        private void CbSapXepTab3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid3.ItemsSource != null)
            {
                var db = new QuanLyCuaHangEntities();
                if (CbSapXepTab3.SelectedItem.ToString() == "Ngày giảm dần")
                {
                    dataGrid3.ItemsSource = db.GiaoDich.
                        Where(hh => hh.NgayGiaoDich >= DPNgayBatDau.SelectedDate &&
                                    hh.NgayGiaoDich <= DPNgayKetThuc.SelectedDate).
                                    OrderByDescending(hh=>hh.NgayGiaoDich).ToList();
                }
                else if (CbSapXepTab3.SelectedItem.ToString() == "Ngày tăng dần")
                {
                    dataGrid3.ItemsSource = db.GiaoDich.
                        Where(hh => hh.NgayGiaoDich >= DPNgayBatDau.SelectedDate &&
                                    hh.NgayGiaoDich <= DPNgayKetThuc.SelectedDate).
                                    OrderBy(hh=>hh.NgayGiaoDich).ToList();
                }
                else
                {
                    dataGrid3.ItemsSource = db.GiaoDich.
                        Where(hh => hh.NgayGiaoDich >= DPNgayBatDau.SelectedDate &&
                                    hh.NgayGiaoDich <= DPNgayKetThuc.SelectedDate).ToList();
                }
            }
        }


        /// <summary>
        /// Hàm dùng để xuất báo cáo khi click vào button "Xuất báo cáo"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnXuatBaoCao_Click(object sender, RoutedEventArgs e)
        {
            if (DPNgayBatDau.Text == "" || DPNgayKetThuc.Text == "")
            {
                MessageBox.Show("Vui lòng chọn khoảng thời gian (từ ngày ... đến ngày ...)");
            }
            else
            {
                if(DPNgayKetThuc.SelectedDate<DPNgayBatDau.SelectedDate)
                {
                    MessageBox.Show("Vui lòng chọn lại khoảng thời gian!");
                }
                else
                {
                    string ThongBao = "(Từ " + DPNgayBatDau.Text + " Đến " + DPNgayKetThuc.Text + ")";
                    TblKhoangThoiGian.Text = ThongBao;

                    var db = new QuanLyCuaHangEntities();
                    dataGrid3.ItemsSource = db.GiaoDich.
                        Where(hh => hh.NgayGiaoDich >= DPNgayBatDau.SelectedDate &&
                                    hh.NgayGiaoDich <= DPNgayKetThuc.SelectedDate).ToList();

                    var TongDoanhThu = db.GiaoDich.Where(hh => hh.NgayGiaoDich >= DPNgayBatDau.SelectedDate &&
                                    hh.NgayGiaoDich <= DPNgayKetThuc.SelectedDate).Sum(hh => hh.ThanhTien);
                    TblTongDoanhThu.Text = TongDoanhThu.ToString();

                    var TongHangHoa=db.GiaoDich.Where(hh => hh.NgayGiaoDich >= DPNgayBatDau.SelectedDate &&
                                    hh.NgayGiaoDich <= DPNgayKetThuc.SelectedDate).Sum(hh => hh.SoLuong);
                    TblTongSoHangHoa.Text = TongHangHoa.ToString();

                    dataGrid4.ItemsSource = db.Procedure_Lay10HangHoaBanChay(DPNgayBatDau.SelectedDate, DPNgayKetThuc.SelectedDate);
                }               
            }
        }


        /// <summary>
        /// Hàm dùng để xuất bảng báo cáo ra excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnXuatExcel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid3.ItemsSource == null)
            {
                MessageBox.Show("Chưa có dữ liệu để xuất!!!");
            }
            else
            {
                //Đoạn code xuất ra Excel tham khảo từ:
                //http://www.yazilimkodlama.com/programlama/wpf-datagrid-icindeki-verileri-excele-aktarma/

                //Tạo 1 bảng excel
                Excel.Application excel = new Excel.Application();
                excel.Visible = true;
                Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Worksheet sheet1 = (Worksheet)workbook.Sheets[1];
                
                //Ghi vào tên các cột
                for (int j = 0; j < dataGrid3.Columns.Count; j++)
                {
                    Range myRange = (Range)sheet1.Cells[1, j + 1];
                    sheet1.Cells[1, j + 1].Font.Bold = true;
                    sheet1.Columns[j + 1].ColumnWidth = 15;
                    myRange.Value2 = dataGrid3.Columns[j].Header;
                }

                //Ghi dữ liệu vào từng cột
                for (int i = 0; i < dataGrid3.Columns.Count; i++)
                {
                    for (int j = 0; j < dataGrid3.Items.Count; j++)
                    {
                        var b = dataGrid3.Columns[i].GetCellContent(dataGrid3.Items[j]) as TextBlock;
                        Microsoft.Office.Interop.Excel.Range myRange = 
                            (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                        if (b != null)
                        {
                            myRange.Value2 = b.Text;
                        }

                    }
                }

                MessageBox.Show("Xuất dữ liệu ra Excel thành công");
            }
        }

        
    }
}
