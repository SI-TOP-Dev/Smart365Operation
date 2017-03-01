using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public class CustomerDetailsViewModel:BindableBase
    {


        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set { SetProperty(ref _companyName, value); }
        }

        private string _companyProfile;
        public string CompanyProfile
        {
            get { return _companyProfile; }
            set { SetProperty(ref _companyProfile, value); }
        }

        private string _contacts;
        public string Contacts
        {
            get { return _contacts; }
            set { SetProperty(ref _contacts, value); }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        private string _industryType;
        public string IndustryType
        {
            get { return _industryType; }
            set { SetProperty(ref _industryType, value); }
        }


            
    }
}
