using DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClientViewer
{
    public partial class TestWebApiForm : Form
    {
        public TestWebApiForm()
        {
            InitializeComponent();

            var jsonModel = new JsonModel()
            {
                BjModel = new BjModel()
                {
                    ID = "testBjId",
                    Nic = "testBjNic"
                },
                UserModels = new List<UserModel>()
                {
                    new UserModel()
                    {
                        ID = "testUserId",
                        Nic = "testUserNic",
                    }
                }
            };

            WebApiCaller.Get(jsonModel);
        }
    }
}
