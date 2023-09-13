using System.Windows.Controls;

namespace RevitSyncExcel.View
{
    public partial class ImporterView : UserControl
    {
        public ImporterView()
        {
            InitializeComponent();

            // Thêm giá trị vào ListBox 1
            listBox1.Items.Add("Mục 1");
            listBox1.Items.Add("Mục 2");
            listBox1.Items.Add("Mục 3");

            // Thêm giá trị vào ListBox 2
            listBox2.Items.Add("Item A");
            listBox2.Items.Add("Item B");
            listBox2.Items.Add("Item C");
        }
    }
}
