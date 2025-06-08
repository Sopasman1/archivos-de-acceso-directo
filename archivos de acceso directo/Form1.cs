namespace archivos_de_acceso_directo
{
    public partial class Form1 : Form
    {
        private GameFileHandler fileHandler;
        private string filePath = "games1.dat";
        public Form1()
        {
            InitializeComponent();
            fileHandler = new GameFileHandler(filePath);

            // Configure ComboBox with common brands
            cmbBrand.Items.AddRange(new string[] {
            "Sony", "Microsoft", "Nintendo", "EA", "Ubisoft",
            "Activision", "Square Enix", "Capcom", "Sega", "Bandai Namco"
        });

            RefreshList();
        }

        private void RefreshList()
        {
            lstGames.DataSource = null;
            lstGames.DataSource = fileHandler.GetAll();
            lstGames.DisplayMember = "ToString";
        }

        private void brnAdd_Click(object sender, EventArgs e)
        {
            if (cmbBrand.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please fill all fields correctly");
                return;
            }

            Game newGame = new Game
            {
                Id = fileHandler.RecordCount() + 1,
                Brand = cmbBrand.SelectedItem.ToString(),
                Name = txtName.Text,
                Price = price
            };

            fileHandler.Add(newGame);
            RefreshList();
            ClearFields();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lstGames.SelectedItem == null) return;

            Game selected = (Game)lstGames.SelectedItem;
            int index = fileHandler.GetAll().IndexOf(selected);

            if (cmbBrand.SelectedIndex == -1 || string.IsNullOrWhiteSpace(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Please fill all fields correctly");
                return;
            }

            Game updated = new Game
            {
                Id = selected.Id,
                Brand = cmbBrand.SelectedItem.ToString(),
                Name = txtName.Text,
                Price = price
            };

            fileHandler.Update(index, updated);
            RefreshList();
            ClearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstGames.SelectedItem == null) return;

            Game selected = (Game)lstGames.SelectedItem;
            int index = fileHandler.GetAll().IndexOf(selected);

            fileHandler.Delete(index);
            RefreshList();
            ClearFields();
        }

        private void lstGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstGames.SelectedItem == null) return;

            Game selected = (Game)lstGames.SelectedItem;

            cmbBrand.SelectedItem = selected.Brand;
            txtName.Text = selected.Name;
            txtPrice.Text = selected.Price.ToString("0.00");
        }

        private void ClearFields()
        {
            cmbBrand.SelectedIndex = -1;
            txtName.Clear();
            txtPrice.Clear();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            var results = fileHandler.GetAll()
                .Where(g => g.Brand.ToLower().Contains(searchTerm) ||
                           g.Name.ToLower().Contains(searchTerm))
                .ToList();

            lstGames.DataSource = null;
            lstGames.DataSource = results;
            lstGames.DisplayMember = "ToString";
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            RefreshList();
        }
    }
}
