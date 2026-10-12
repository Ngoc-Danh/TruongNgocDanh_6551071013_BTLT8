using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport;
using FastReport.Export.Pdf;
using Microsoft.EntityFrameworkCore;
using Cau2_QuanCaPhe.Models;

namespace Cau2_QuanCaPhe;

public partial class FormMain : Form
{
    private QuanCaPheDbContext _context = null!;

    // Top Filter Controls
    private GroupBox grpTimKiem = null!;
    private DateTimePicker dtpTuNgay = null!;
    private DateTimePicker dtpDenNgay = null!;
    private ComboBox cboLocBan = null!;
    private ComboBox cboLocHinhThuc = null!;
    private TextBox txtLocTenMon = null!;
    private Button btnTimKiem = null!;

    // Left Panel: Danh sách hóa đơn
    private GroupBox grpDanhSachHD = null!;
    private DataGridView dgvHoaDon = null!;
    private Label lblTongDoanhThu = null!;
    private Button btnHoaDonMoi = null!;
    private Button btnSuaHoaDon = null!;
    private Button btnXoaHoaDon = null!;

    // Right Panel: Chi tiết hóa đơn
    private GroupBox grpChiTietHD = null!;
    private DateTimePicker dtpNgayLap = null!;
    private ComboBox cboSoBan = null!;
    private RadioButton rdoTienMat = null!;
    private RadioButton rdoChuyenKhoan = null!;
    private TextBox txtGhiChu = null!;

    // Thêm món controls
    private ComboBox cboChonMon = null!;
    private NumericUpDown numSoLuong = null!;
    private Button btnThemMon = null!;

    // Chi tiết Grid & Total
    private DataGridView dgvChiTiet = null!;
    private Label lblTongCong = null!;

    // Right action buttons
    private Button btnSuaSL = null!;
    private Button btnXoaMon = null!;
    private Button btnXemHoaDon = null!;
    private Button btnXuatPdf = null!;
    private Button btnIn = null!;

    // State
    private int? _selectedMaHD = null;
    private int? _selectedMaCT = null;
    private List<Mon> _danhSachMon = new();

    public FormMain()
    {
        _context = new QuanCaPheDbContext();
        InitializeComponentCustom();
    }

    private void InitializeComponentCustom()
    {
        this.Text = "Quán cà phê Góc Phố - Quản lý hóa đơn";
        this.Size = new Size(1180, 720);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        this.MinimumSize = new Size(1050, 650);

        // ================= 1. GroupBox Tìm kiếm hóa đơn =================
        grpTimKiem = new GroupBox
        {
            Text = "Tìm kiếm hóa đơn",
            Location = new Point(12, 10),
            Size = new Size(1140, 75),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        var lblTuNgay = new Label { Text = "Từ ngày", Location = new Point(15, 20), AutoSize = true };
        dtpTuNgay = new DateTimePicker
        {
            Location = new Point(15, 40),
            Size = new Size(125, 23),
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy",
            Value = DateTime.Now.AddMonths(-1)
        };

        var lblDenNgay = new Label { Text = "Đến ngày", Location = new Point(155, 20), AutoSize = true };
        dtpDenNgay = new DateTimePicker
        {
            Location = new Point(155, 40),
            Size = new Size(125, 23),
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy",
            Value = DateTime.Now.AddDays(1)
        };

        var lblLocBan = new Label { Text = "Số bàn", Location = new Point(295, 20), AutoSize = true };
        cboLocBan = new ComboBox
        {
            Location = new Point(295, 40),
            Size = new Size(100, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cboLocBan.Items.Add("Tất cả");
        for (int i = 1; i <= 10; i++) cboLocBan.Items.Add($"Bàn {i}");
        cboLocBan.SelectedIndex = 0;

        var lblLocHT = new Label { Text = "Hình thức", Location = new Point(410, 20), AutoSize = true };
        cboLocHinhThuc = new ComboBox
        {
            Location = new Point(410, 40),
            Size = new Size(130, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cboLocHinhThuc.Items.AddRange(new object[] { "Tất cả", "Tiền mặt", "Chuyển khoản" });
        cboLocHinhThuc.SelectedIndex = 0;

        var lblLocMon = new Label { Text = "Tên món", Location = new Point(555, 20), AutoSize = true };
        txtLocTenMon = new TextBox
        {
            Location = new Point(555, 40),
            Size = new Size(260, 23),
            PlaceholderText = "Nhập tên món cần tìm..."
        };

        btnTimKiem = new Button
        {
            Text = "Tìm",
            Location = new Point(830, 38),
            Size = new Size(95, 27)
        };

        grpTimKiem.Controls.AddRange(new Control[] {
            lblTuNgay, dtpTuNgay, lblDenNgay, dtpDenNgay,
            lblLocBan, cboLocBan, lblLocHT, cboLocHinhThuc,
            lblLocMon, txtLocTenMon, btnTimKiem
        });

        // ================= 2. Vùng trái: Danh sách hóa đơn =================
        grpDanhSachHD = new GroupBox
        {
            Text = "Danh sách hóa đơn",
            Location = new Point(12, 95),
            Size = new Size(540, 565),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
        };

        dgvHoaDon = new DataGridView
        {
            Location = new Point(10, 25),
            Size = new Size(520, 440),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            RowHeadersWidth = 25
        };

        lblTongDoanhThu = new Label
        {
            Text = "Tổng doanh thu đang hiển thị: 0 đ",
            Location = new Point(10, 475),
            Size = new Size(520, 28),
            Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.DarkBlue,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left
        };

        btnHoaDonMoi = new Button { Text = "Hóa đơn mới", Location = new Point(10, 515), Size = new Size(115, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        btnSuaHoaDon = new Button { Text = "Sửa hóa đơn", Location = new Point(135, 515), Size = new Size(115, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        btnXoaHoaDon = new Button { Text = "Xóa hóa đơn", Location = new Point(260, 515), Size = new Size(115, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };

        grpDanhSachHD.Controls.AddRange(new Control[] {
            dgvHoaDon, lblTongDoanhThu,
            btnHoaDonMoi, btnSuaHoaDon, btnXoaHoaDon
        });

        // ================= 3. Vùng phải: Chi tiết hóa đơn =================
        grpChiTietHD = new GroupBox
        {
            Text = "Chi tiết hóa đơn",
            Location = new Point(565, 95),
            Size = new Size(585, 565),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };

        // Form fields
        dtpNgayLap = new DateTimePicker
        {
            Location = new Point(15, 25),
            Size = new Size(160, 23),
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "dd/MM/yyyy HH:mm"
        };

        var lblBan = new Label { Text = "Số bàn:", Location = new Point(185, 27), AutoSize = true };
        cboSoBan = new ComboBox
        {
            Location = new Point(235, 24),
            Size = new Size(70, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        for (int i = 1; i <= 10; i++) cboSoBan.Items.Add(i);
        cboSoBan.SelectedIndex = 0;

        rdoTienMat = new RadioButton { Text = "Tiền mặt", Location = new Point(325, 25), AutoSize = true, Checked = true };
        rdoChuyenKhoan = new RadioButton { Text = "Chuyển khoản", Location = new Point(415, 25), AutoSize = true };

        var lblGhiChu = new Label { Text = "Ghi chú:", Location = new Point(15, 58), AutoSize = true };
        txtGhiChu = new TextBox
        {
            Location = new Point(70, 55),
            Size = new Size(480, 42),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        // Thêm món controls
        var lblChonMon = new Label { Text = "Chọn món:", Location = new Point(15, 108), AutoSize = true };
        cboChonMon = new ComboBox
        {
            Location = new Point(85, 105),
            Size = new Size(240, 23),
            DropDownStyle = ComboBoxStyle.DropDownList,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };

        var lblSL = new Label { Text = "SL:", Location = new Point(335, 108), AutoSize = true, Anchor = AnchorStyles.Top | AnchorStyles.Right };
        numSoLuong = new NumericUpDown
        {
            Location = new Point(365, 105),
            Size = new Size(60, 23),
            Minimum = 1,
            Maximum = 50,
            Value = 1,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        btnThemMon = new Button
        {
            Text = "Thêm món",
            Location = new Point(440, 103),
            Size = new Size(110, 27),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        // DataGridView chi tiết
        dgvChiTiet = new DataGridView
        {
            Location = new Point(15, 142),
            Size = new Size(535, 323),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            RowHeadersWidth = 25
        };

        lblTongCong = new Label
        {
            Text = "Tổng cộng: 0 đ",
            Location = new Point(15, 475),
            Size = new Size(535, 28),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point),
            ForeColor = Color.DarkRed,
            TextAlign = ContentAlignment.MiddleRight,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
        };

        btnSuaSL = new Button { Text = "Sửa SL", Location = new Point(15, 515), Size = new Size(85, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        btnXoaMon = new Button { Text = "Xóa món", Location = new Point(105, 515), Size = new Size(95, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        btnXemHoaDon = new Button { Text = "Xem hóa đơn", Location = new Point(205, 515), Size = new Size(115, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        btnXuatPdf = new Button { Text = "Xuất PDF", Location = new Point(325, 515), Size = new Size(95, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        btnIn = new Button { Text = "In", Location = new Point(425, 515), Size = new Size(75, 34), Anchor = AnchorStyles.Bottom | AnchorStyles.Left };

        grpChiTietHD.Controls.AddRange(new Control[] {
            dtpNgayLap, lblBan, cboSoBan, rdoTienMat, rdoChuyenKhoan,
            lblGhiChu, txtGhiChu,
            lblChonMon, cboChonMon, lblSL, numSoLuong, btnThemMon,
            dgvChiTiet, lblTongCong,
            btnSuaSL, btnXoaMon, btnXemHoaDon, btnXuatPdf, btnIn
        });

        this.Controls.AddRange(new Control[] { grpTimKiem, grpDanhSachHD, grpChiTietHD });

        // Event hooks
        this.Load += FormMain_Load;
        btnTimKiem.Click += async (s, e) => await LoadHoaDonListAsync();
        txtLocTenMon.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; await LoadHoaDonListAsync(); } };

        dgvHoaDon.SelectionChanged += DgvHoaDon_SelectionChanged;
        dgvChiTiet.SelectionChanged += DgvChiTiet_SelectionChanged;

        btnHoaDonMoi.Click += BtnHoaDonMoi_Click;
        btnSuaHoaDon.Click += BtnSuaHoaDon_Click;
        btnXoaHoaDon.Click += BtnXoaHoaDon_Click;

        btnThemMon.Click += BtnThemMon_Click;
        btnSuaSL.Click += BtnSuaSL_Click;
        btnXoaMon.Click += BtnXoaMon_Click;

        btnXemHoaDon.Click += BtnXemHoaDon_Click;
        btnXuatPdf.Click += BtnXuatPdf_Click;
        btnIn.Click += BtnIn_Click;
    }

    private bool _isLoaded = false;

    private async void FormMain_Load(object? sender, EventArgs e)
    {
        try
        {
            if (_isLoaded) return;
            SetupGrids();
            await LoadDanhSachMonAsync();
            await LoadHoaDonListAsync(1);
            _isLoaded = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi kết nối CSDL: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetupGrids()
    {
        // Grid Hóa đơn
        dgvHoaDon.AutoGenerateColumns = false;
        dgvHoaDon.Columns.Clear();

        dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "MaHdHienThi",
            HeaderText = "Mã HD",
            FillWeight = 85
        });

        var colNgay = new DataGridViewTextBoxColumn
        {
            DataPropertyName = "NgayLap",
            HeaderText = "Ngày lập",
            FillWeight = 115
        };
        colNgay.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        dgvHoaDon.Columns.Add(colNgay);

        dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "SoBanHienThi",
            HeaderText = "Số bàn",
            FillWeight = 60
        });

        dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "HinhThucTt",
            HeaderText = "Hình thức TT",
            FillWeight = 95
        });

        var colTongTien = new DataGridViewTextBoxColumn
        {
            DataPropertyName = "TongTien",
            HeaderText = "Tổng tiền",
            FillWeight = 95
        };
        colTongTien.DefaultCellStyle.Format = "N0";
        colTongTien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvHoaDon.Columns.Add(colTongTien);

        // Grid Chi tiết
        dgvChiTiet.AutoGenerateColumns = false;
        dgvChiTiet.Columns.Clear();

        dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "STT",
            HeaderText = "STT",
            FillWeight = 35
        });

        dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = "TenMon",
            HeaderText = "Tên món",
            FillWeight = 160
        });

        var colSL = new DataGridViewTextBoxColumn
        {
            DataPropertyName = "SoLuong",
            HeaderText = "SL",
            FillWeight = 45
        };
        colSL.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        dgvChiTiet.Columns.Add(colSL);

        var colDonGia = new DataGridViewTextBoxColumn
        {
            DataPropertyName = "DonGia",
            HeaderText = "Đơn giá",
            FillWeight = 80
        };
        colDonGia.DefaultCellStyle.Format = "N0";
        colDonGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvChiTiet.Columns.Add(colDonGia);

        var colThanhTien = new DataGridViewTextBoxColumn
        {
            DataPropertyName = "ThanhTien",
            HeaderText = "Thành tiền",
            FillWeight = 90
        };
        colThanhTien.DefaultCellStyle.Format = "N0";
        colThanhTien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dgvChiTiet.Columns.Add(colThanhTien);
    }

    private async Task LoadDanhSachMonAsync()
    {
        _danhSachMon = await _context.Mons.OrderBy(m => m.TenMon).ToListAsync();
        cboChonMon.Items.Clear();
        foreach (var m in _danhSachMon)
        {
            cboChonMon.Items.Add($"{m.TenMon} ({m.DonGia:N0} đ)");
        }
        if (cboChonMon.Items.Count > 0) cboChonMon.SelectedIndex = 0;
    }

    public class HoaDonRowViewModel
    {
        public int MaHd { get; set; }
        public string MaHdHienThi => $"HD{MaHd:D6}";
        public DateTime NgayLap { get; set; }
        public int? SoBan { get; set; }
        public string SoBanHienThi => SoBan.HasValue ? SoBan.Value.ToString() : "";
        public string HinhThucTt { get; set; } = string.Empty;
        public decimal TongTien { get; set; }
        public string? GhiChu { get; set; }
    }

    private async Task LoadHoaDonListAsync(int? selectMaHD = null)
    {
        try
        {
            var query = _context.HoaDons.Include(h => h.ChiTietHoaDons).ThenInclude(ct => ct.MaMonNavigation).AsQueryable();

            // Lọc theo khoảng ngày
            DateTime tu = dtpTuNgay.Value.Date;
            DateTime den = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(h => h.NgayLap >= tu && h.NgayLap <= den);

            // Lọc theo số bàn
            if (cboLocBan.SelectedIndex > 0)
            {
                int ban = cboLocBan.SelectedIndex; // 1 to 10
                query = query.Where(h => h.SoBan == ban);
            }

            // Lọc theo hình thức thanh toán
            if (cboLocHinhThuc.SelectedIndex > 0)
            {
                string ht = cboLocHinhThuc.SelectedItem?.ToString() ?? "";
                query = query.Where(h => h.HinhThucTt == ht);
            }

            // Lọc theo tên món
            string keywordMon = txtLocTenMon.Text.Trim();
            if (!string.IsNullOrWhiteSpace(keywordMon))
            {
                query = query.Where(h => h.ChiTietHoaDons.Any(ct => ct.MaMonNavigation.TenMon.Contains(keywordMon)));
            }

            var list = await query.OrderByDescending(h => h.NgayLap).ToListAsync();

            var viewList = list.Select(h => new HoaDonRowViewModel
            {
                MaHd = h.MaHd,
                NgayLap = h.NgayLap,
                SoBan = h.SoBan,
                HinhThucTt = h.HinhThucTt,
                TongTien = h.ChiTietHoaDons.Sum(ct => (decimal)ct.SoLuong * ct.DonGia),
                GhiChu = h.GhiChu
            }).ToList();

            dgvHoaDon.DataSource = viewList;

            decimal tongDoanhThu = viewList.Sum(v => v.TongTien);
            lblTongDoanhThu.Text = $"Tổng doanh thu đang hiển thị: {tongDoanhThu:N0} đ";

            if (selectMaHD.HasValue)
            {
                for (int i = 0; i < dgvHoaDon.Rows.Count; i++)
                {
                    if (dgvHoaDon.Rows[i].DataBoundItem is HoaDonRowViewModel row && row.MaHd == selectMaHD.Value)
                    {
                        dgvHoaDon.Rows[i].Selected = true;
                        dgvHoaDon.CurrentCell = dgvHoaDon.Rows[i].Cells[0];
                        break;
                    }
                }
            }
            else if (dgvHoaDon.Rows.Count > 0)
            {
                dgvHoaDon.Rows[0].Selected = true;
            }
            else
            {
                ClearChiTietView();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải danh sách hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ClearChiTietView()
    {
        _selectedMaHD = null;
        _selectedMaCT = null;
        grpChiTietHD.Text = "Chi tiết hóa đơn";
        dgvChiTiet.DataSource = null;
        lblTongCong.Text = "Tổng cộng: 0 đ";
        txtGhiChu.Clear();
    }

    private async void DgvHoaDon_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvHoaDon.CurrentRow?.DataBoundItem is HoaDonRowViewModel row)
        {
            _selectedMaHD = row.MaHd;
            grpChiTietHD.Text = $"Chi tiết hóa đơn {row.MaHdHienThi}";
            dtpNgayLap.Value = row.NgayLap;
            if (row.SoBan.HasValue && row.SoBan.Value >= 1 && row.SoBan.Value <= 10)
            {
                cboSoBan.SelectedIndex = row.SoBan.Value - 1;
            }
            if (row.HinhThucTt == "Tiền mặt") rdoTienMat.Checked = true;
            else rdoChuyenKhoan.Checked = true;
            txtGhiChu.Text = row.GhiChu ?? "";

            await LoadChiTietHoaDonAsync(row.MaHd);
        }
    }

    public class ChiTietRowViewModel
    {
        public int MaCt { get; set; }
        public int MaMon { get; set; }
        public int STT { get; set; }
        public string TenMon { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien => SoLuong * DonGia;
    }

    private async Task LoadChiTietHoaDonAsync(int maHd)
    {
        try
        {
            var chiTiets = await _context.ChiTietHoaDons
                .Include(ct => ct.MaMonNavigation)
                .Where(ct => ct.MaHd == maHd)
                .OrderBy(ct => ct.MaCt)
                .ToListAsync();

            var list = new List<ChiTietRowViewModel>();
            for (int i = 0; i < chiTiets.Count; i++)
            {
                var ct = chiTiets[i];
                list.Add(new ChiTietRowViewModel
                {
                    MaCt = ct.MaCt,
                    MaMon = ct.MaMon,
                    STT = i + 1,
                    TenMon = ct.MaMonNavigation?.TenMon ?? "",
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia
                });
            }

            dgvChiTiet.DataSource = list;

            decimal tongCong = list.Sum(x => x.ThanhTien);
            lblTongCong.Text = $"Tổng cộng: {tongCong:N0} đ";

            // Cập nhật lại tổng tiền trên lưới trái
            if (dgvHoaDon.CurrentRow?.DataBoundItem is HoaDonRowViewModel hdRow && hdRow.MaHd == maHd)
            {
                hdRow.TongTien = tongCong;
                dgvHoaDon.InvalidateRow(dgvHoaDon.CurrentRow.Index);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tải chi tiết hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void DgvChiTiet_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvChiTiet.CurrentRow?.DataBoundItem is ChiTietRowViewModel ct)
        {
            _selectedMaCT = ct.MaCt;
            numSoLuong.Value = Math.Clamp(ct.SoLuong, 1, 50);
            for (int i = 0; i < _danhSachMon.Count; i++)
            {
                if (_danhSachMon[i].MaMon == ct.MaMon)
                {
                    cboChonMon.SelectedIndex = i;
                    break;
                }
            }
        }
        else
        {
            _selectedMaCT = null;
        }
    }

    // ================= CRUD HÓA ĐƠN =================
    private async void BtnHoaDonMoi_Click(object? sender, EventArgs e)
    {
        try
        {
            var hd = new HoaDon
            {
                NgayLap = dtpNgayLap.Value,
                SoBan = cboSoBan.SelectedIndex + 1,
                HinhThucTt = rdoTienMat.Checked ? "Tiền mặt" : "Chuyển khoản",
                GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim()
            };

            _context.HoaDons.Add(hd);
            await _context.SaveChangesAsync();
            await LoadHoaDonListAsync(hd.MaHd);
            MessageBox.Show($"Đã tạo mới hóa đơn HD{hd.MaHd:D6} thành công! Bây giờ bạn có thể thêm món vào hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi tạo hóa đơn mới: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnSuaHoaDon_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!_selectedMaHD.HasValue)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var hd = await _context.HoaDons.FindAsync(_selectedMaHD.Value);
            if (hd == null) return;

            hd.NgayLap = dtpNgayLap.Value;
            hd.SoBan = cboSoBan.SelectedIndex + 1;
            hd.HinhThucTt = rdoTienMat.Checked ? "Tiền mặt" : "Chuyển khoản";
            hd.GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim();

            await _context.SaveChangesAsync();
            await LoadHoaDonListAsync(_selectedMaHD.Value);
            MessageBox.Show($"Cập nhật thông tin hóa đơn HD{hd.MaHd:D6} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi sửa hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnXoaHoaDon_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!_selectedMaHD.HasValue)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maHd = _selectedMaHD.Value;
            var xacNhan = MessageBox.Show($"Bạn có chắc chắn muốn xóa hóa đơn HD{maHd:D6} và toàn bộ các dòng chi tiết không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes) return;

            var hd = await _context.HoaDons.Include(h => h.ChiTietHoaDons).FirstOrDefaultAsync(h => h.MaHd == maHd);
            if (hd != null)
            {
                // Xóa chi tiết trước rồi mới xóa hóa đơn
                _context.ChiTietHoaDons.RemoveRange(hd.ChiTietHoaDons);
                _context.HoaDons.Remove(hd);
                await _context.SaveChangesAsync();
                await LoadHoaDonListAsync();
                MessageBox.Show($"Đã xóa hóa đơn HD{maHd:D6} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi xóa hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ================= CRUD CHI TIẾT HÓA ĐƠN =================
    private async void BtnThemMon_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!_selectedMaHD.HasValue)
            {
                MessageBox.Show("Vui lòng chọn hoặc tạo hóa đơn trước khi thêm món!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboChonMon.SelectedIndex < 0 || cboChonMon.SelectedIndex >= _danhSachMon.Count)
            {
                MessageBox.Show("Vui lòng chọn món!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var monDuocChon = _danhSachMon[cboChonMon.SelectedIndex];
            int soLuongThem = (int)numSoLuong.Value;

            // Kiểm tra xem món này đã có trong hóa đơn chưa
            var chiTietHienCo = await _context.ChiTietHoaDons
                .FirstOrDefaultAsync(ct => ct.MaHd == _selectedMaHD.Value && ct.MaMon == monDuocChon.MaMon);

            if (chiTietHienCo != null)
            {
                chiTietHienCo.SoLuong += soLuongThem;
            }
            else
            {
                var ctMoi = new ChiTietHoaDon
                {
                    MaHd = _selectedMaHD.Value,
                    MaMon = monDuocChon.MaMon,
                    SoLuong = soLuongThem,
                    DonGia = monDuocChon.DonGia ?? 0
                };
                _context.ChiTietHoaDons.Add(ctMoi);
            }

            await _context.SaveChangesAsync();
            await LoadChiTietHoaDonAsync(_selectedMaHD.Value);
            // Cập nhật lại tổng doanh thu dưới lưới trái
            if (dgvHoaDon.DataSource is List<HoaDonRowViewModel> viewList)
            {
                lblTongDoanhThu.Text = $"Tổng doanh thu đang hiển thị: {viewList.Sum(v => v.TongTien):N0} đ";
            }
            MessageBox.Show($"Đã thêm món '{monDuocChon.TenMon}' vào hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi thêm món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnSuaSL_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!_selectedMaCT.HasValue || !_selectedMaHD.HasValue)
            {
                MessageBox.Show("Vui lòng chọn món trong hóa đơn cần sửa số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ct = await _context.ChiTietHoaDons.FindAsync(_selectedMaCT.Value);
            if (ct == null) return;

            ct.SoLuong = (int)numSoLuong.Value;
            await _context.SaveChangesAsync();
            await LoadChiTietHoaDonAsync(_selectedMaHD.Value);

            if (dgvHoaDon.DataSource is List<HoaDonRowViewModel> viewList)
            {
                lblTongDoanhThu.Text = $"Tổng doanh thu đang hiển thị: {viewList.Sum(v => v.TongTien):N0} đ";
            }
            MessageBox.Show("Đã cập nhật số lượng món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi cập nhật số lượng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnXoaMon_Click(object? sender, EventArgs e)
    {
        try
        {
            if (!_selectedMaCT.HasValue || !_selectedMaHD.HasValue)
            {
                MessageBox.Show("Vui lòng chọn món trong hóa đơn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ct = await _context.ChiTietHoaDons.Include(c => c.MaMonNavigation).FirstOrDefaultAsync(c => c.MaCt == _selectedMaCT.Value);
            if (ct == null) return;

            var xacNhan = MessageBox.Show($"Bạn có chắc muốn xóa món '{ct.MaMonNavigation?.TenMon}' khỏi hóa đơn?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (xacNhan != DialogResult.Yes) return;

            _context.ChiTietHoaDons.Remove(ct);
            await _context.SaveChangesAsync();
            await LoadChiTietHoaDonAsync(_selectedMaHD.Value);

            if (dgvHoaDon.DataSource is List<HoaDonRowViewModel> viewList)
            {
                lblTongDoanhThu.Text = $"Tổng doanh thu đang hiển thị: {viewList.Sum(v => v.TongTien):N0} đ";
            }
            MessageBox.Show("Đã xóa món khỏi hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi xóa món: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ================= REPORT FASTREPORT =================
    private string GetFrxPath()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "HoaDon.frx");
        if (File.Exists(path)) return path;

        path = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "HoaDon.frx");
        if (File.Exists(path)) return path;

        var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        while (dir != null)
        {
            string check = Path.Combine(dir.FullName, "Reports", "HoaDon.frx");
            if (File.Exists(check)) return check;
            dir = dir.Parent;
        }

        throw new FileNotFoundException("Không tìm thấy file mẫu HoaDon.frx!");
    }

    private async Task<(Report? report, HoaDon? hd, List<ChiTietIn> listIn)> ChuanBiReportHoaDonAsync()
    {
        if (!_selectedMaHD.HasValue)
        {
            MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return (null, null, new());
        }

        var hd = await _context.HoaDons
            .Include(h => h.ChiTietHoaDons)
            .ThenInclude(ct => ct.MaMonNavigation)
            .FirstOrDefaultAsync(h => h.MaHd == _selectedMaHD.Value);

        if (hd == null || hd.ChiTietHoaDons.Count == 0)
        {
            MessageBox.Show("Chưa chọn hóa đơn hoặc hóa đơn chưa có món để in!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return (null, null, new());
        }

        var listIn = new List<ChiTietIn>();
        int stt = 1;
        foreach (var ct in hd.ChiTietHoaDons.OrderBy(c => c.MaCt))
        {
            listIn.Add(new ChiTietIn
            {
                STT = stt++,
                TenMon = ct.MaMonNavigation?.TenMon ?? "",
                SoLuong = ct.SoLuong,
                DonGia = ct.DonGia,
                ThanhTien = (decimal)ct.SoLuong * ct.DonGia
            });
        }

        string frxPath = GetFrxPath();
        var report = new Report();
        report.Load(frxPath);

        // Đăng ký danh sách ChiTiet
        report.RegisterData(listIn, "ChiTiet");
        report.GetDataSource("ChiTiet").Enabled = true;

        // Truyền các tham số
        report.SetParameterValue("TenQuan", "QUÁN CÀ PHÊ GÓC PHỐ");
        report.SetParameterValue("SoHD", $"HD{hd.MaHd:D6}");
        report.SetParameterValue("NgayLap", hd.NgayLap.ToString("dd/MM/yyyy HH:mm"));
        report.SetParameterValue("SoBan", $"Bàn: {hd.SoBan}");
        report.SetParameterValue("HinhThucTT", $"Hình thức: {hd.HinhThucTt}");

        return (report, hd, listIn);
    }

    private async void BtnXemHoaDon_Click(object? sender, EventArgs e)
    {
        try
        {
            var (report, hd, listIn) = await ChuanBiReportHoaDonAsync();
            if (report == null) return;

            using (report)
            {
                if (report.Prepare())
                {
                    report.ShowPrepared(true);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi hiển thị hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnXuatPdf_Click(object? sender, EventArgs e)
    {
        try
        {
            var (report, hd, listIn) = await ChuanBiReportHoaDonAsync();
            if (report == null || hd == null) return;

            using (report)
            {
                string defaultFileName = $"HoaDon_HD{hd.MaHd:D6}.pdf";
                using var sfd = new SaveFileDialog
                {
                    Filter = "PDF Files (*.pdf)|*.pdf",
                    FileName = defaultFileName,
                    Title = "Xuất hóa đơn ra PDF"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    report.Prepare();
                    var pdfExport = new PDFExport();
                    report.Export(pdfExport, sfd.FileName);

                    var hoi = MessageBox.Show($"Xuất hóa đơn HD{hd.MaHd:D6} ra PDF thành công! Bạn có muốn mở file không?",
                        "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (hoi == DialogResult.Yes)
                    {
                        Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi xuất PDF hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void BtnIn_Click(object? sender, EventArgs e)
    {
        try
        {
            var (report, hd, listIn) = await ChuanBiReportHoaDonAsync();
            if (report == null) return;

            using (report)
            {
                report.Prepare();
                report.PrintSettings.ShowDialog = true;
                report.Print();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi in hóa đơn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public async Task RunAutomatedTestAsync()
    {
        while (!_isLoaded)
        {
            await Task.Delay(100);
        }
        _selectedMaHD = 1;
        await LoadChiTietHoaDonAsync(1);
        for (int i = 0; i < dgvHoaDon.Rows.Count; i++)
        {
            if (dgvHoaDon.Rows[i].DataBoundItem is HoaDonRowViewModel r && r.MaHd == 1)
            {
                dgvHoaDon.Rows[i].Selected = true;
                break;
            }
        }
        await Task.Delay(500);
        string projectDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));

        // Chụp ảnh giao diện chính HD000001
        CaptureFormScreenshot(Path.Combine(projectDir, "KetQua_Cau2_GiaoDien.png"));

        // Lọc theo tên món: tiramisu
        txtLocTenMon.Text = "tiramisu";
        await LoadHoaDonListAsync();
        await Task.Delay(500);
        CaptureFormScreenshot(Path.Combine(projectDir, "KetQua_Cau2_LocTiramisu.png"));

        // Lọc theo hình thức: Chuyển khoản
        txtLocTenMon.Text = "";
        cboLocHinhThuc.SelectedItem = "Chuyển khoản";
        await LoadHoaDonListAsync();
        await Task.Delay(500);
        CaptureFormScreenshot(Path.Combine(projectDir, "KetQua_Cau2_LocChuyenKhoan.png"));

        // Reset bộ lọc và xuất PDF mẫu HD000001
        cboLocHinhThuc.SelectedIndex = 0;
        await LoadHoaDonListAsync(1);
        _selectedMaHD = 1;
        await LoadChiTietHoaDonAsync(1);
        await Task.Delay(400);

        try
        {
            var (report, hd, listIn) = await ChuanBiReportHoaDonAsync();
            if (report != null)
            {
                using (report)
                {
                    report.Prepare();
                    var pdfExport = new PDFExport();
                    report.Export(pdfExport, Path.Combine(projectDir, "HoaDon_HD000001.pdf"));
                }
            }
        }
        catch { }
    }

    private void CaptureFormScreenshot(string filePath)
    {
        try
        {
            using var bmp = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bmp, new Rectangle(0, 0, this.Width, this.Height));
            bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }
        catch { }
    }
}
