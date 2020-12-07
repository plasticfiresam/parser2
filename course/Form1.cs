using course.Entities;
using course.Parser;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace course
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (Data.ApplicationContext db = new Data.ApplicationContext())
            {
                try
            {
                var parser = new SiteParser();
                var productsTask = await parser.ParseHomePageProducts();

                var products = productsTask;
                var category = new Entities.ProductCategory() { Name = "Ноутбуки" };

                
                    db.ProductCategories.Add(category);
                    db.SaveChanges();

                    var sellers = new List<Seller>();
                    foreach (var product in products)
                    {
                        if (product.Seller != null)
                            sellers.Add(product.Seller);
                        product.ProductCategory = category;
                    }
                    db.Sellers.AddRange(sellers);
                    db.Products.AddRange(products);
                    db.SaveChanges();
                    ShowProductList();

                }
                catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                }
            }
        }
        private static void ShowProductList()
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "parserDataSet.Sellers". При необходимости она может быть перемещена или удалена.
            this.sellersTableAdapter.Fill(this.parserDataSet.Sellers);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "parserDataSet.Products". При необходимости она может быть перемещена или удалена.
            this.productsTableAdapter.Fill(this.parserDataSet.Products);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "parserDataSet.ProductCategories". При необходимости она может быть перемещена или удалена.
            this.productCategoriesTableAdapter.Fill(this.parserDataSet.ProductCategories);

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
