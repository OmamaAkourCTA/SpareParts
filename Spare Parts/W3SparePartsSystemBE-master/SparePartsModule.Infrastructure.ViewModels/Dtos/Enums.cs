using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparePartsModule.Infrastructure.ViewModels.Dtos.Enums
{
   
    public enum Status
    {
        Active = 1001,
        Inactive =1002,
        Deleted=1005

    }
    public enum Settings
    {
        Group=3001,
        verageLeadTime=0,
        MaxDiscount=100
    }
    public enum LookupTypes
    {
        Status = 1,//master looup
        Country = 2,//master lookup
        Group = 3,//master
        Company = 4,//master
        TaxType = 5,//master,
        DiscountType = 6,//master
        Currency = 7,//master
        UnitofMeasurement = 8,//master,
        ModelCodeType=2,
        PaymentMethod = 4,
        DeliveryMethod = 5,
        Itemcategorytype = 7,
        MinMaxType = 8,
        ItemCategorySerialType = 9,

        ItemNameCode = 3,
        Warehouse = 12,
        LocationZone = 13,
        LocationShelf = 14,
        LocationWHSection = 15,
        ItemNameType = 16,
        SupplierRegion = 17,
        SupplierFlage=18,
        OrderFlage=19,
        SalesFlage=20,
        LocationType=21,
        SubstituteType=22,
        RejectionReason= 24,
        ReviseReason=25,
        OrderType = 27,
        OrderMethod = 28,
        OrderFrieght = 29,

        //vechile
        MasterSpecsCategory =9,
        WarehouseType=34,
        OrderSource=41,
        SubstituteCode=26



    }
    public enum StatusAdmin
    {
        Active = 2001,
        Inactive = 2002,
        Pending = 2003,
        Approved = 2004,
        Closed = 2005,
        Rejected = 2006,
        Deleted = 2007
    }
    public enum RegisterStatus
    {
        Opened = 16001,
        Unclosed = 16002,
        Waiting = 16003,
        Closed = 16004,
        Partial = 16005,
        Settled = 16006
    }
    public enum Portals
    {
        POS = 17001,
        CreditManagement = 17002,
        ServiceIntermediateLayer = 17003,
        Treasury = 17004,
        SparepartsIntermediateLayers = 17005,
        DirectPaymentIntermediateLayer = 17006,
        Admin = 17007
    }
    public enum Roles
    {
        CasherRegister = 1,
        SuperCashier = 2,
        PartsSalesConsultant = 3,
        ServiceAdvisor = 32,
        SuperAdmin = 36,
        TreasuryAdmin = 37,
        MainFund = 40,
        PettyCashCustodian = 41

    }
    public enum Permissions
    {
        OpenCloseRegister = 11,
        CollectRegisterYes = 15,
        TreasuryViewManage = 95
    }
    public enum BundelStatus
    {
        Pending = 23001,
        Approved = 23002,
        Rejected = 23003,
        NeedRevision = 23004,
        Draft = 23005,
        Inactive= 30008,
        Expired=23006
    }
    public enum OrderStatus
    {
        Draft = 30001,
        Pending = 30002,
        Approved = 30003,
        Rejected = 30004,
        Ordered = 30005,
        Confirmed = 30006,
        NeedRevision = 30007,
        Processing=30008,
        Processed=30009,
        LookedForPermissions= 30010,
        PendingConfirmation=30011,
    }
    public enum POSSStatus
    {
        Uploaded = 31001,
    }
    public enum SubstituteTypes
    {
        OMultiSubstitute=22001,
        Substitute = 22002,
        Alternative= 22003


    }
   public enum Lookups
    {
        DUB=19001,
        ExceedMinMax=32001,
        HighFOB=32002,
        HighWeight=32003,
        HighVolume=32004,
        Mixed=32005,
        ExceedMinMaxLibrary=32006,
    }

}
