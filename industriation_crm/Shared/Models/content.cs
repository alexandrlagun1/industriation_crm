using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace industriation_crm.Shared.Models
{
    public class user
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public int? role_id { get; set; }
        public string? password { get; set; }
        public string? login { get; set; }

        [ForeignKey("role_id")]
        public virtual roles? roles { get; set; }
        public virtual List<client>? clients { get; set; }
    }
    public class roles
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }

    }
    public class contact
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? patronymic { get; set; }
        public string? surname { get; set; }
        public string? full_name { get; set; }
        public string? email { get; set; }
        public int? client_id { get; set; }
        public int? main_contact { get; set; }
        public string? phone { get; set; }
        public client? client { get; set; }
        public List<contact_phone>? contact_phones { get; set; } = new();
    }
    public class client
    {

        [Key]
        public int id { get; set; }
        public int? client_type { get; set; }
        public int? user_id { get; set; }
        public string? org_name { get; set; }
        public string? org_address { get; set; }
        public long? org_inn { get; set; }
        public long? org_kpp { get; set; }
        public long? org_ogrn { get; set; }
        public long? bank_bik { get; set; }
        public long? bank_cor_schet { get; set; }
        public long? bank_ras_schet { get; set; }
        public string? bank_name { get; set; }

        [ForeignKey("client_id")]
        public List<contact>? contacts { get; set; }

        [ForeignKey("user_id")]
        public user? user { get; set; }

        public List<order>? orders { get; set; }

        [NotMapped]
        public contact? main_contact { get; set; } = new();

    }
    public class product
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public double? price { get; set; }
    }
    public class order
    {
        [Key]
        public int id { get; set; }
        public int? client_id { get; set; }
        public int? user_id { get; set; }
        public int? order_status_id { get; set; }
        public int? main_contact_id { get; set; }
        public int? delivery_id { get; set; }
        public string? comments { get; set; }
        public int? percent_discount { get; set; } = 0;
        [NotMapped]
        public int? _percent_discount
        {
            get
            {
                return percent_discount;
            }
            set
            {

                if (product_To_Orders != null && product_To_Orders.Count != 0)
                {
                    if (percent_discount == null)
                        percent_discount = 0;
                    if (value == null)
                        product_To_Orders?.ForEach(p => p.discount_total_price = p.discount_total_price - (p.total_price / 100 * percent_discount));
                    else
                        product_To_Orders?.ForEach(p => p.discount_total_price = (p.discount_total_price - (p.total_price / 100 * percent_discount)) + p.total_price / 100 * value);
                }
                percent_discount = value;
            }
        }
        public double? ruble_discount { get; set; } = 0;
        [NotMapped]
        public double? _ruble_discount
        {
            get
            {
                return ruble_discount;
            }
            set
            {
                if (product_To_Orders != null && product_To_Orders.Count != 0)
                {
                    if (ruble_discount == null)
                        ruble_discount = 0;
                    if (value == null)
                        product_To_Orders?.ForEach(p => p.discount_total_price = p.discount_total_price - ruble_discount);
                    else
                    {
                        product_To_Orders?.ForEach(p => p.discount_total_price = (p.discount_total_price - ruble_discount) + value);
                    }
                }
                ruble_discount = value;
            }
        }

        public double? price_summ { get; set; }
        [NotMapped]
        public double? _price_summ
        {
            get
            {
                double? product_summ = product_To_Orders?.Select(p => p._product_price * p._count).Sum();
                price_summ = product_summ + delivery?.price! - _current_pay_summ;
                return price_summ;
            }
            set
            {
                price_summ = value;
                if (value < 0)
                    price_summ = 0;

            }
        }
        public double? current_pay_summ { get; set; }

        [NotMapped]
        public double? _current_pay_summ
        {
            get
            {
                if (order_Pays != null && order_Pays.Count != 0)
                {
                    current_pay_summ = order_Pays.Where(o => o.pay_status_id == 2 && o.isRemove == false).Select(o => o.price).Sum();
                    return current_pay_summ;
                }
                current_pay_summ = 0;
                return current_pay_summ;
            }
        }

        [ForeignKey("delivery_id")]
        public delivery? delivery { get; set; } = new();

        [ForeignKey("main_contact_id")]
        public contact? main_contact { get; set; }

        [ForeignKey("client_id")]
        public client? client { get; set; }

        [ForeignKey("user_id")]
        public user? user { get; set; }

        [ForeignKey("order_status_id")]
        public order_status? order_status { get; set; }
        public List<product_to_order>? product_To_Orders { get; set; }
        public List<order_pay>? order_Pays { get; set; } = new();
    }
    public class order_status
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
    }
    public class product_to_order
    {
        [Key]
        public int id { get; set; }
        public int? count { get; set; }

        [NotMapped]
        public int? _count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                total_price = product_price * value;
            }
        }
        public int? product_id { get; set; }
        public int? order_id { get; set; }
        public int? supplier_order_id { get; set; }
        public double? discount_total_price { get; set; } = 0;

        public double? product_price { get; set; }

        [NotMapped]
        public double? _product_price
        {
            get
            {
                return product_price;
            }
            set
            {
                product_price = value;
                total_price = count * value;
            }
        }
        public double? total_price { get; set; } = 0;
        public int? product_postition { get; set; } = 0;
        public int? delivery_period { get; set; }
        public int? delivery_period_type_id { get; set; }
        public double? discount { get; set; }

        [ForeignKey("product_id")]
        public product? product { get; set; }

        [ForeignKey("order_id")]
        public order? order { get; set; }

        [NotMapped]
        public order? _order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        [ForeignKey("supplier_order_id")]
        public supplier_order? supplier_order { get; set; }

        [ForeignKey("delivery_period_type_id")]
        public delivery_period_type? delivery_period_type { get; set; }


    }
    public class supplier
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public List<supplier_order>? supplier_Orders { get; set; }
    }
    public class supplier_order
    {
        [Key]
        public int id { get; set; }
        public int? supplier_id { get; set; }
        public int? user_id { get; set; }

        [ForeignKey("supplier_id")]
        public supplier? supplier { get; set; }

        [ForeignKey("user_id")]
        public user? user { get; set; }

        public List<product_to_order>? product_to_orders { get; set; }
    }
    public class delivery
    {
        [Key]
        public int id { get; set; }
        public DateTime? shipment_date { get; set; }
        public int? delivery_type_id { get; set; }
        public string? recipient_name { get; set; }
        public string? recipient_phone { get; set; }
        public int? order_id { get; set; }
        public double? price { get; set; } = 0;
        public string? address { get; set; }

        [ForeignKey("order_id")]
        public order? order { get; set; }
        [ForeignKey("delivery_type_id")]
        public delivery_type? delivery_type { get; set; }
    }
    public class delivery_type
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public double? price { get; set; }
    }
    public class order_pay
    {
        [Key]
        public int id { get; set; }
        public double? price { get; set; }
        public int? order_id { get; set; }
        public int? pay_status_id { get; set; }
        public DateTime? date { get; set; }

        [ForeignKey("pay_status_id")]
        public pay_status? pay_Status { get; set; }

        [ForeignKey("order_id")]
        public order? order { get; set; }
        [NotMapped]
        public bool isRemove { get; set; }
    }
    public class pay_status
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
    }
    public class delivery_period_type
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
    }
    public class contact_phone
    {
        [Key]
        public int id { get; set; }
        public string? phone { get; set; }
        public int? contact_id { get; set; }

        [ForeignKey("contact_id")]
        public contact? contact { get; set; }

        [NotMapped]
        public bool isRemove { get; set; }
    }
}
