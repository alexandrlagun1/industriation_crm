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
        public string? phone { get; set; }
        public string? megafon_login { get; set; }

        [ForeignKey("role_id")]
        public virtual roles? roles { get; set; }
        public virtual List<client>? clients { get; set; }
    }
    public class user_notifications
    {
        [Key]
        public int id { get; set; }
        public DateTime? date { get; set; }
        public int? user_id { get; set; }
        public string? text { get; set; }
    }
    public class task
    {
        [Key]
        public int id { get; set; }
        public string text { get; set; }
        public int complete { get; set; }
        public int? creator_id { get; set; }
        public int? executor_id { get; set; }
        public DateTime? date { get; set; } = DateTime.Now;
        public int order_id { get; set; }
        [ForeignKey("creator_id")]
        public user? creator { get; set; }
        [ForeignKey("executor_id")]
        public user? executor { get; set; }
    }
    public class stage
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
    public class order_history
    {
        [Key]
        public int id { get; set; }
        public string text { get; set; }
        public DateTime date { get; set; }
        public int order_id { get; set; }

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
        public int? is_active { get; set; }
        public client? client { get; set; }
        public List<contact_phone>? contact_phones { get; set; } = new();
    }
    public class call_history
    {
        [Key]
        public int id { get; set; }
        public string? client_number { get; set; }
        public string? manager_number { get; set; }
        public string? duration { get; set; }
        public string? status { get; set; }
        public string? call_id { get; set; }
        public string? type { get; set; }
        public string? record { get; set; }
        public int? contact_id { get; set; }
        public int? user_id { get; set; }
        [ForeignKey("user_id")]
        public user? user { get; set; }
        [ForeignKey("contact_id")]
        public contact? contact { get; set; }
        public DateTime? date_time { get; set; }
    }
    public class client
    {
        [Key]
        public int id { get; set; }
        public int? client_type { get; set; }
        public int? user_id { get; set; }
        public string? org_name { get; set; }
        public string? org_address { get; set; }
        public int is_supplier { get; set; }
        public long? org_inn { get; set; }
        public long? org_kpp { get; set; }
        public long? org_ogrn { get; set; }
        public long? bank_bik { get; set; }
        public long? bank_cor_schet { get; set; }
        public long? bank_ras_schet { get; set; }
        public string? bank_name { get; set; }
        public DateTime? add_date { get; set; } = DateTime.Now;

        [ForeignKey("client_id")]
        public List<contact>? contacts { get; set; }

        [ForeignKey("user_id")]
        public user? user { get; set; }

        public List<order>? orders { get; set; }

    }
    public class category
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public int parent_id { get; set; }
    }
    public class pay_status
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
    }
    public class product
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public double? price { get; set; }
        public int? category_id { get; set; }
        public int? external_id { get; set; }
        public string? image { get; set; }
        public string? article { get; set; }
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
        public int? percent_discount { get; set; }
        public double? ruble_discount { get; set; }
        public double? price_summ { get; set; }
        public DateTime? order_date { get; set; } = DateTime.Now;
        public string? notes { get; set; }
        public int? stage_id { get; set; }
        public int? pay_status_id { get; set; }
        public int? supplier_manager_id { get; set; }
        [ForeignKey("supplier_manager_id")]
        public user? supplier_manager { get; set; }

        [ForeignKey("pay_status_id")]
        public pay_status? pay_status { get; set; }
        [ForeignKey("stage_id")]
        public stage? stage { get; set; }
        [ForeignKey("order_id")]
        public List<order_history> order_Histories { get; set; } = new();
        [ForeignKey("order_id")]
        public List<task> tasks { get; set; } = new();

        [NotMapped]
        public int? _percent_discount
        {
            get
            {
                if (percent_discount != null)
                    return percent_discount;
                else
                    return 0;
            }
            set
            {
                if (value != null)
                    percent_discount = value;
                else
                    percent_discount = 0;
                product_To_Orders?.ForEach(p => p.order_percent_discount = value);
            }
        }
        [NotMapped]
        public double? _ruble_discount
        {
            get
            {
                if (ruble_discount != null)
                    return ruble_discount;
                else
                    return 0;
            }
            set
            {
                if (value != null)
                    ruble_discount = value;
                else
                    ruble_discount = 0;
                product_To_Orders?.ForEach(p => p.order_ruble_discount = value);
            }
        }
        [NotMapped]
        public double? products_with_delivery_price
        {
            get
            {
                if (delivery.price == null)
                    return _total_price;
                return delivery.price + _total_price;
            }
        }


        [NotMapped]
        public double? _price_summ
        {
            get
            {
                if (delivery?.price == null)
                    price_summ = _total_price - _current_pay_summ;
                else
                    price_summ = _total_price + delivery?.price - _current_pay_summ;
                if (price_summ < 0)
                    price_summ = 0;
                return price_summ;
            }
            set
            {
                price_summ = value;
                if (value < 0)
                    price_summ = 0;

            }
        }
        [NotMapped]
        public double? _total_price
        {
            get
            {
                if (product_To_Orders == null)
                    return null;
                else
                    return product_To_Orders?.Select(p => p._total_price).Sum();
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
                    current_pay_summ = order_Pays.Where(o => o.isRemove == false).Select(o => o.price).Sum();
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
        [NotMapped]
        public bool id_delete_from_supplier_order { get; set; }
        [NotMapped]
        public bool is_add_to_supplier_order { get; set; }
        [NotMapped]
        public bool showPriceModal = false;
        [Key]
        public int id { get; set; }
        public int? count { get; set; }
        public int? product_id { get; set; }
        public int? order_id { get; set; }
        public int? supplier_order_id { get; set; }
        public double? product_price { get; set; }
        public int? product_postition { get; set; } = 0;
        public int? delivery_period { get; set; }
        public int? delivery_period_type_id { get; set; }
        public double? ruble_discount { get; set; }
        public int? percent_discount { get; set; }
        public int? supplier_delivery_period { get; set; }
        public double? supplier_price { get; set; }
        [ForeignKey("product_id")]
        public product? product { get; set; }

        [ForeignKey("order_id")]
        public order? order { get; set; }

        [ForeignKey("supplier_order_id")]
        public supplier_order? supplier_order { get; set; }

        [ForeignKey("delivery_period_type_id")]
        public delivery_period_type? delivery_period_type { get; set; }

        private double? total_price
        {
            get
            {
                double result = Math.Round(Convert.ToDouble(_product_price_with_discount * _count), 2);
                return result;
            }
        }
        [NotMapped]
        public double? _total_price
        {
            get
            {
                double result = Math.Round(Convert.ToDouble(_product_price_with_discount * _count - _discount_total_price), 2);
                if (result < 0)
                    return 0;
                return result;
            }
        }

        private double? _order_ruble_discount;
        [NotMapped]
        public double? order_ruble_discount
        {
            get
            {
                if (_order_ruble_discount == null)
                    return 0;
                else
                    return _order_ruble_discount;
            }
            set
            {
                if (value == null)
                    _order_ruble_discount = 0;
                else
                    _order_ruble_discount = value;
            }
        }

        private double? _order_percent_discount;
        [NotMapped]
        public double? order_percent_discount
        {
            get
            {
                if (_order_percent_discount == null)
                    return 0;
                else
                    return _order_percent_discount;
            }
            set
            {
                if (value == null)
                    _order_percent_discount = 0;
                else
                    _order_percent_discount = value;
            }
        }

        [NotMapped]
        public double? _discount_total_price
        {
            get
            {
                double result = Math.Round(Convert.ToDouble(order_ruble_discount + (total_price / 100 * order_percent_discount)), 2);
                return result;
            }
        }

        [NotMapped]
        public double? _product_price_with_discount
        {
            get
            {
                if (ruble_discount == null)
                    ruble_discount = 0;
                if (percent_discount == null)
                    percent_discount = 0;
                double result = Convert.ToDouble(product_price - (product_price / 100 * percent_discount) - ruble_discount);
                return Math.Round(result, 2);
            }
        }

        [NotMapped]
        public int? _count
        {
            get
            {
                if (count != null)
                    return count;
                else
                    return 0;
            }
            set
            {
                count = value;
            }
        }
        [NotMapped]
        public double? _product_price
        {
            get
            {
                if (product_price != null)
                    return product_price;
                else
                    return 0;
            }
            set
            {
                product_price = value;
            }
        }

    }

    public class supplier_order
    {
        [Key]
        public int id { get; set; }
        public int? supplier_id { get; set; }
        public int? user_id { get; set; }
        public DateTime? date { get; set; } = DateTime.Now;

        [ForeignKey("user_id")]
        public user? user { get; set; }

        [ForeignKey("supplier_id")]
        public client? supplier { get; set; } = new();
        public List<product_to_order>? product_to_orders { get; set; } = new();
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
        [NotMapped]
        public double? _price
        {
            get
            {
                if (price == null)
                    return 0;
                return price;
            }
            set
            {
                if (value == null)
                    price = 0;
                else
                    price = value;
            }
        }
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
        public DateTime? date { get; set; }
        [ForeignKey("order_id")]
        public order? order { get; set; }
        [NotMapped]
        public bool isRemove { get; set; }
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
