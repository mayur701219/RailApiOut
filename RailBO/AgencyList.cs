using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rail.BO
{
    #region All Agencies Fetch

    [Serializable()]
    public partial class AgencyList
    {
        [JsonProperty("IsSuccess", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSuccess { get; set; }

        [JsonProperty("Errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<Error> Errors { get; set; }

        [JsonProperty("Data", NullValueHandling = NullValueHandling.Ignore)]
        public List<AgenciesData> Data { get; set; }
    }
    [Serializable()]
    public partial class AgenciesData
    {
        [JsonProperty("AgencyID", NullValueHandling = NullValueHandling.Ignore)]
        public long? AgencyId { get; set; }

        [JsonProperty("AgencyName", NullValueHandling = NullValueHandling.Ignore)]
        public string AgencyName { get; set; }

        [JsonProperty("Icust", NullValueHandling = NullValueHandling.Ignore)]
        public string Icust { get; set; }

        [JsonProperty("DisplayName", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("UserName", NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [JsonProperty("UserTypeID", NullValueHandling = NullValueHandling.Ignore)]
        public int UserTypeId { get; set; }

        [JsonProperty("Groupid", NullValueHandling = NullValueHandling.Ignore)]
        public long? Groupid { get; set; }

        [JsonProperty("AgentCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentCountry { get; set; }

        [JsonProperty("AgentCountryText", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentCountryText { get; set; }

        [JsonProperty("AgentNationality", NullValueHandling = NullValueHandling.Ignore)]
        public long? AgentNationality { get; set; }

        [JsonProperty("AgentNationalityText", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentNationalityText { get; set; }
    }
    [Serializable()]
    public partial class Error
    {
        [JsonProperty("Key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        [JsonProperty("Value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("Agencyid", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AgencyId { get; set; }
    }

    public partial class AgencyList
    {
        public static AgencyList FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AgencyList>(json, Converter.Settings);
        }
    }

    #endregion

    #region Selected Agency Details
    [Serializable()]
    public partial class AgencyDetails
    {
        [JsonProperty("IsSuccess", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSuccess { get; set; }

        [JsonProperty("Errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<AgencyError> Errors { get; set; }

        [JsonProperty("Data", NullValueHandling = NullValueHandling.Ignore)]
        public Agency Agency { get; set; }
        public string? CorrelationId { get; set; }
        public string? HasAccess { get; set; }
    }
    [Serializable()]
    public partial class Agency
    {
        [JsonProperty("MyProperty", NullValueHandling = NullValueHandling.Ignore)]
        public MyProperty MyProperty { get; set; }

        [JsonProperty("LocationCode", NullValueHandling = NullValueHandling.Ignore)]
        public string LocationCode { get; set; }

        [JsonProperty("ErpAgentBalance", NullValueHandling = NullValueHandling.Ignore)]
        public long? ErpAgentBalance { get; set; }

        [JsonProperty("blockstatus", NullValueHandling = NullValueHandling.Ignore)]
        public string Blockstatus { get; set; }

        [JsonProperty("UserBalance")]
        public long? UserBalance { get; set; }
        public string UserMobileNo { get; set; }
        public string UserEmailID { get; set; }
        public bool IsAutoTicketing { get; set; }
        public bool IsCancelRequest { get; set; }
        public bool IsSelfBalance { get; set; }
        public List<AllowUserBooking> AllowRiyaUserBooking { get; set; }
    }
    [Serializable()]
    public partial class MyProperty
    {
        [JsonProperty("AgencyID", NullValueHandling = NullValueHandling.Ignore)]
        public long? AgencyId { get; set; }

        [JsonProperty("BookingCountry", NullValueHandling = NullValueHandling.Ignore)]
        public string BookingCountry { get; set; }

        [JsonProperty("AgencyName", NullValueHandling = NullValueHandling.Ignore)]
        public string AgencyName { get; set; }

        [JsonProperty("Icast", NullValueHandling = NullValueHandling.Ignore)]
        public string Icast { get; set; }

        [JsonProperty("DisplayName", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("AgentBalance", NullValueHandling = NullValueHandling.Ignore)]
        public long? AgentBalance { get; set; }

        [JsonProperty("AgentROE", NullValueHandling = NullValueHandling.Ignore)]
        public long? AgentRoe { get; set; }

        [JsonProperty("BaseCurrency", NullValueHandling = NullValueHandling.Ignore)]
        public string BaseCurrency { get; set; }

        [JsonProperty("Currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("OfficeID", NullValueHandling = NullValueHandling.Ignore)]
        public string OfficeId { get; set; }

        [JsonProperty("AgentMobileNo", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentMobileNo { get; set; }

        [JsonProperty("AgentEmailID", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentEmailID { get; set; }

        [JsonProperty("BranchCode", NullValueHandling = NullValueHandling.Ignore)]
        public string BranchCode { get; set; }

        [JsonProperty("FirstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("LastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("MobileNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string MobileNumber { get; set; }

        [JsonProperty("HomeNo", NullValueHandling = NullValueHandling.Ignore)]
        public string HomeNo { get; set; }

        [JsonProperty("Address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty("City", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        [JsonProperty("Country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("AgentNationality", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentNationality { get; set; }

        [JsonProperty("AgentNationalityText", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentNationalityText { get; set; }

        [JsonProperty("Pincode", NullValueHandling = NullValueHandling.Ignore)]
        public string? Pincode { get; set; }

        [JsonProperty("MyProperty")]
        public object MyPropertyMyProperty { get; set; }

        [JsonProperty("Province", NullValueHandling = NullValueHandling.Ignore)]
        public string Province { get; set; }

        [JsonProperty("AgentApproved", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AgentApproved { get; set; }

        [JsonProperty("STATUS", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Status { get; set; }

        [JsonProperty("AutoTicketing", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoTicketing { get; set; }

        [JsonProperty("ParentAgentID", NullValueHandling = NullValueHandling.Ignore)]
        public long? ParentAgentId { get; set; }

        [JsonProperty("UserType", NullValueHandling = NullValueHandling.Ignore)]
        public long? UserType { get; set; }

        [JsonProperty("BalanceUpdateDate", NullValueHandling = NullValueHandling.Ignore)]
        public long? BalanceUpdateDate { get; set; }

        [JsonProperty("CrypticContactNo", NullValueHandling = NullValueHandling.Ignore)]
        public string CrypticContactNo { get; set; }
        [JsonProperty("AllowAgentBooking", NullValueHandling = NullValueHandling.Ignore)]
        public List<AllowUserBooking> AllowAgentBooking { get; set; }
    }
    [Serializable()]
    public partial class AgencyError
    {
        [JsonProperty("Agencyid", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> AgencyId { get; set; }
    }

    public partial class AgencyDetails
    {
        public static AgencyDetails FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AgencyDetails>(json, Converter.Settings);
        }
    }
    [Serializable()]
    public partial class AgentBalanceDetails
    {
        [JsonProperty("IsSuccess", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSuccess { get; set; }

        [JsonProperty("Errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<AgencyError> Errors { get; set; }

        [JsonProperty("Data", NullValueHandling = NullValueHandling.Ignore)]
        public AgentBalance Agency { get; set; }
    }
    [Serializable()]
    public partial class AgentBalance
    {
        public string Balance { get; set; }
    }
    [Serializable()]
    public class AllowUserBooking
    {
        public string Product { get; set; }
        public string AllowBooking { get; set; }
    }
    public partial class AgentBalanceDetails
    {
        public static AgentBalanceDetails FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AgentBalanceDetails>(json, Converter.Settings);
        }
    }
    #endregion

    #region Service Fee Details
    [Serializable()]
    public partial class ServiceFeeList
    {
        [JsonProperty("IsSuccess", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSuccess { get; set; }

        [JsonProperty("Errors", NullValueHandling = NullValueHandling.Ignore)]
        public List<Error> Errors { get; set; }

        [JsonProperty("Data", NullValueHandling = NullValueHandling.Ignore)]
        public List<ServiceFee> ServiceFee { get; set; }
    }
    [Serializable()]
    public partial class ServiceFee
    {
        [JsonProperty("Id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("ServiceFeesAppliedId", NullValueHandling = NullValueHandling.Ignore)]
        public int ServiceFeesAppliedId { get; set; }

        [JsonProperty("MarketPoint", NullValueHandling = NullValueHandling.Ignore)]
        public string MarketPoint { get; set; }

        [JsonProperty("UserType", NullValueHandling = NullValueHandling.Ignore)]
        public string UserType { get; set; }

        [JsonProperty("IssuanceFrom", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime IssuanceFrom { get; set; }

        [JsonProperty("IssuanceTo", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime IssuanceTo { get; set; }

        [JsonProperty("Origin", NullValueHandling = NullValueHandling.Ignore)]
        public object Origin { get; set; }

        [JsonProperty("Destination", NullValueHandling = NullValueHandling.Ignore)]
        public string Destination { get; set; }

        [JsonProperty("GST", NullValueHandling = NullValueHandling.Ignore)]
        public object GST { get; set; }

        [JsonProperty("ServiceType", NullValueHandling = NullValueHandling.Ignore)]
        public string ServiceType { get; set; }

        [JsonProperty("isActive", NullValueHandling = NullValueHandling.Ignore)]
        public bool isActive { get; set; }

        [JsonProperty("CreatedDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("ModifiedDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("CreatedBy", NullValueHandling = NullValueHandling.Ignore)]
        public object CreatedBy { get; set; }

        [JsonProperty("ModifyBy", NullValueHandling = NullValueHandling.Ignore)]
        public object ModifyBy { get; set; }

        [JsonProperty("AgentId", NullValueHandling = NullValueHandling.Ignore)]
        public string AgentId { get; set; }

        [JsonProperty("Fk_SupplierMasterId", NullValueHandling = NullValueHandling.Ignore)]
        public int Fk_SupplierMasterId { get; set; }

        [JsonProperty("ProductType", NullValueHandling = NullValueHandling.Ignore)]
        public string ProductType { get; set; }

        [JsonProperty("ProductName", NullValueHandling = NullValueHandling.Ignore)]
        public string ProductName { get; set; }

        [JsonProperty("BookingFee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? BookingFee { get; set; }

        [JsonProperty("FK_ServiceFeeId", NullValueHandling = NullValueHandling.Ignore)]
        public int FK_ServiceFeeId { get; set; }

        [JsonProperty("Fk_ProductListMasterId", NullValueHandling = NullValueHandling.Ignore)]
        public int Fk_ProductListMasterId { get; set; }

        [JsonProperty("Currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }

        [JsonProperty("Commission", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Commission { get; set; }

        [JsonProperty("AdditionAmount", NullValueHandling = NullValueHandling.Ignore)]
        public decimal AdditionAmount { get; set; }

        [JsonProperty("MarkUpOnBookingFee", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MarkUpOnBookingFee { get; set; }

        [JsonProperty("ProductTypeName", NullValueHandling = NullValueHandling.Ignore)]
        public string ProductTypeName { get; set; }
    }

    public partial class ServiceFee
    {
        public static List<ServiceFee> FromJson(string Json)
        {
            return JsonConvert.DeserializeObject<List<ServiceFee>>(Json, Converter.Settings);
        }
    }
    public partial class ServiceFeeList
    {
        public static ServiceFeeList FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ServiceFeeList>(json, Converter.Settings);
        }
    }
    #endregion

    #region Current selected agent IMP properties
    [Serializable()]
    public class CurrentAgent
    {
        public string AgencyId { get; set; }
        public string Text { get; set; }
        public string SelfBalance { get; set; }
        public string CreditLimit { get; set; }
        public string Currency { get; set; }
        public string Country { get; set; }
        public string AgentMobile { get; set; }
        public string AgentEmailId { get; set; }
        public string RiyaUserId { get; set; }
        public string RiyaUserMobile { get; set; }
        public string RiyaEmailID { get; set; }
        public string LoggedinUserName { get; set; }
        public string DeviceId { get; set; }
        public decimal MarkUpBookingFees { get; set; }
        public string HasAccess { get; set; }
        public string AllowBooking { get; set; }
    }
    #endregion

}
