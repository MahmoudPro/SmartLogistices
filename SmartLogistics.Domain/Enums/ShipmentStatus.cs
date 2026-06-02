
namespace SmartLogistics.Domain.Enums
{
    public enum ShipmentStatus
    {
        Submitted = 1,      // أنشأه العميل
        Confirmed = 2,      // وافق عليه الموظف
        Processing = 3,     // قيد التجهيز وتعيين سائق
        InTransit = 4,      // الشاحنة في الطريق
        OutForDelivery = 5, // السائق عند العميل
        Delivered = 6,      // تم التسليم ✅
        Cancelled = 7,      // ملغي
        Returned = 8        // مرتجع
    }
}
