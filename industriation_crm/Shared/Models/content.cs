﻿using System.ComponentModel.DataAnnotations;
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
        public bool? retail_synch { get; set; } = true;
        public string? login { get; set; }
        public string? email { get; set; }
        public int? retail_id { get; set; }
        public string? phone { get; set; }
        public string? megafon_login { get; set; }
        public string? orders_filter { get; set; }
        public string? clients_filter { get; set; }
        public string? products_filter { get; set; }
        public string? call_history_filter { get; set; }
        public string? job_title { get; set; }

        [ForeignKey("role_id")]
        public virtual roles? roles { get; set; }
        public virtual List<client>? clients { get; set; }
    }
    public class progress_client_discount
    {
        [Key]
        public int id { get; set; }
        public int discount { get; set; }
        public int up_point { get; set; }
        public int max_discount { get; set; }

    }
    public class user_notifications
    {
        [Key]
        public int id { get; set; }
        public DateTime? date { get; set; }
        public int? user_id { get; set; }
        public string? text { get; set; }
    }
    public class order_check
    {
        [NotMapped]
        public string _css_class
        {
            get
            {
                if(current == 1)
                {
                    return "score-active";
                }
                else
                {
                    return "score";
                }
            }
        }
        [NotMapped]
        public bool is_add { get; set; }
        [NotMapped]
        public bool is_delete { get; set; }
        [Key]
        public int id { get; set; } = 0;
        public int? percent_discount { get; set; }
        public double? ruble_discount { get; set; }
        public double? price_summ { get; set; }
        public double? current_pay_summ { get; set; }
        public string? currency { get; set; } = "₽";
        public int? current { get; set; } = 0;
        public int? cancelling { get; set; }
        public int? order_id { get; set; }
        public int? is_pay { get; set; }
        public int? check_number { get; set; }
        public int? print_form_id { get; set; }

        [ForeignKey("print_form_id")]
        public print_form_data? print_form_data { get; set; } = new();
        public virtual List<product_to_order>? product_To_Orders { get; set; } = new();
        [ForeignKey("order_id")]
        public virtual order? order { get; set; }
        
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
        public double? _total_product_price
        {
            get
            {
                if (product_To_Orders == null)
                    return null;
                else
                    return Math.Round(Convert.ToDouble(product_To_Orders?.Where(p => p.is_delete_from_order == false).Select(p => p._total_price).Sum()), 2);
            }
        }

    }
    public class task
    {
        [Key]
        public int id { get; set; }
        public bool? is_view { get; set; } = false;
        public string? text { get; set; }
        public int complete { get; set; }
        public int? creator_id { get; set; }
        public int? executor_id { get; set; }
        public int? client_id { get; set; }
        public DateTime? execute_date { get; set; } = DateTime.Now;
        public DateTime? date { get; set; } = DateTime.Now;
        public int? order_id { get; set; }
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
        public string? dop_email { get; set; }
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
        [NotMapped]
        public bool isActive = false;
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
        public int? client_type { get; set; } = 1;
        public int? user_id { get; set; }
        public string? org_name { get; set; }
        public string? org_address { get; set; }
        public int is_supplier { get; set; }
        public long? org_inn { get; set; }
        public long? org_kpp { get; set; }
        public long? org_ogrn { get; set; }
        public string? org_ogrnip { get; set; }
        public string? delivery_adr { get; set; }
        public string? delivery_cont_phone { get; set; }
        public string? delivery_cont { get; set; }
        public long? bank_bik { get; set; }
        public string? bank_cor_schet { get; set; }
        public string? bank_ras_schet { get; set; }
        public string? bank_name { get; set; }
        public string? tag { get; set; }
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
    public class print_form_data
    {
        [Key]
        public int id { get; set; }
        public string? signatory_position_im_pad { get; set; } = "Директор";
        public string? io_surname { get; set; } = "______________________";
        public string? basis_contract { get; set; } = "Устава";
        public string? signatory_position_rod_pad { get; set; } = "Директора";
        public string? signatory_fio_rod_pad { get; set; } = "_________________________________________";
        public string? spec_number { get; set; } = "1";

    }
    public class product
    {
        [Key]
        public int? id { get; set; }
        public string? name { get; set; }
        public double? price { get; set; } = 0;
        public int? category_id { get; set; }
        public int? external_id { get; set; }
        public string? image { get; set; }
        public string? article { get; set; }
        public string? unit { get; set; } = "шт";
        public string? manufacturer { get; set; }
        public int? quantity { get; set; } = 0;
        public string? site_url { get; set; }

    }
    public class order
    {
        [Key]
        public int id { get; set; }
        [NotMapped]
        public bool retail_synchro { get; set; } = true;
        public string? retail_id { get; set; } 
        public string order_status_name 
        { 
            get 
            {
                if (order_status_id == 1)
                    return "td-order-new";
                else if(order_status_id == 2)
                    return "td-v-schet";
                else if (order_status_id == 3)
                    return "td-trade-confirm";
                else if (order_status_id == 4)
                    return "td-peres";
                else if (order_status_id == 5)
                    return "td-part-order";
                else if (order_status_id == 6)
                    return "td-order";
                else if (order_status_id == 7)
                    return "td-cancel";
                else if (order_status_id == 8)
                    return "td-return";
                else if (order_status_id == 9)
                    return "td-on-sklad";
                else if (order_status_id == 10)
                    return "td-to-dost";
                else if (order_status_id == 11)
                    return "td-delivered";
                else if (order_status_id == 12)
                    return "td-close";
                else
                    return "";
            }
            
        }
        public string? retail_sposob_oplaty { get; set; } = "predoplata";
        public string? postoplata_condition { get; set; }
        public int? pay_predoplata_percent { get; set; } 
        public int? second_pay_predoplata_percent { get; set; }
        public string? pay_predoplata_condition { get; set; }
        public string? second_pay_predoplata_condition { get; set; }

        public int? client_id { get; set; }
        public int? user_id { get; set; }
        public int? order_status_id { get; set; } = 1;
        public int? main_contact_id { get; set; }
        public int? delivery_id { get; set; }
        public string? comments { get; set; }
        //public int? percent_discount { get; set; }
        //public double? ruble_discount { get; set; }
        //public double? price_summ { get; set; }
        public double? current_pay_summ { get; set; }
        public DateTime? order_date { get; set; } = DateTime.Now;
        public string? notes { get; set; }
        public int? stage_id { get; set; } = 1;
        public int? pay_status_id { get; set; } = 4;
        public int? pay_conditions { get; set; } = 1;
        
        public int? supplier_manager_id { get; set; }
        public virtual List<order_check>? order_Checks { get; set; } = new();
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
        public order_check? _pay_check
        {
            get
            {

                return order_Checks?.Where(c => c.is_pay == 1).FirstOrDefault();
            }
        }
        [NotMapped]
        public order_check? _current_check
        {
            get
            {
                
                return order_Checks?.Where(c => c.current == 1).FirstOrDefault();
            }
        }
        [NotMapped]
        public double? _price_summ //Сколько осталось оплатить
        {
            get
            {
                if (_current_check != null)
                {
                    if (delivery?.price == null)
                        _current_check.price_summ = _current_check._total_product_price - _current_pay_summ;
                    else
                        _current_check.price_summ = _current_check._total_product_price + delivery?.price - _current_pay_summ;
                    if (_current_check.price_summ < 0)
                        _current_check.price_summ = 0;
                    return _current_check.price_summ;
                }
                return 0;
            }
            set
            {
                if (_current_check != null)
                {
                    _current_check.price_summ = value;
                    if (value < 0)
                        _current_check.price_summ = 0;
                }
            }
        }
        [NotMapped]
        public double? products_with_delivery_price
        {
            get
            {
                if (_current_check != null)
                {
                    if (delivery?.price == null)
                        return _current_check._total_product_price;
                    return delivery.price + _current_check._total_product_price;
                }
                return 0;
            }
        }
        //[NotMapped]
        //public int? _percent_discount
        //{
        //    get
        //    {
        //        if (percent_discount != null)
        //            return percent_discount;
        //        else
        //            return 0;
        //    }
        //    set
        //    {
        //        if (value != null)
        //            percent_discount = value;
        //        else
        //            percent_discount = 0;
        //        product_To_Orders?.ForEach(p => p.order_percent_discount = value);
        //    }
        //}
        //[NotMapped]
        //public double? _ruble_discount
        //{
        //    get
        //    {
        //        if (ruble_discount != null)
        //            return ruble_discount;
        //        else
        //            return 0;
        //    }
        //    set
        //    {
        //        if (value != null)
        //            ruble_discount = value;
        //        else
        //            ruble_discount = 0;
        //        product_To_Orders?.ForEach(p => p.order_ruble_discount = value);
        //    }
        //}
        //[NotMapped]
        //public double? products_with_delivery_price
        //{
        //    get
        //    {
        //        if (delivery.price == null)
        //            return _total_product_price;
        //        return delivery.price + _total_product_price;
        //    }
        //}


        //[NotMapped]
        //public double? _price_summ
        //{
        //    get
        //    {
        //        if (delivery?.price == null)
        //            price_summ = _total_product_price - _current_pay_summ;
        //        else
        //            price_summ = _total_product_price + delivery?.price - _current_pay_summ;
        //        if (price_summ < 0)
        //            price_summ = 0;
        //        return price_summ;
        //    }
        //    set
        //    {
        //        price_summ = value;
        //        if (value < 0)
        //            price_summ = 0;

        //    }
        //}
        //[NotMapped]
        //public double? _total_product_price
        //{
        //    get
        //    {
        //        if (product_To_Orders == null)
        //            return null;
        //        else
        //            return product_To_Orders?.Select(p => p._total_product_price).Sum();
        //    }
        //}



        [NotMapped]
        public double? _current_pay_summ //Всего оплачено
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
        //public List<product_to_order>? product_To_Orders { get; set; }
        public List<order_pay>? order_Pays { get; set; } = new();
    }
    public class order_status
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? retail_status { get; set; }
        public string? style{ get; set; }
        [NotMapped]
        public bool is_check { get; set; }
    }
    public class product_to_order
    {
        
        [NotMapped]
        public string tr_class { get; set; } = "";
        [NotMapped]
        public bool is_add_from_order { get; set; }
        [NotMapped]
        public bool is_delete_from_order { get; set; }
        [NotMapped]
        public bool is_delete_from_supplier_order { get; set; }
        [NotMapped]
        public bool is_add_to_supplier_order { get; set; }
        [NotMapped]
        public bool showPriceModal = false;
        [Key]
        public int id { get; set; }
        public int? count { get; set; }
        public int? product_id { get; set; }
        public int? supplier_order_id { get; set; }
        public double? product_price { get; set; }
        [NotMapped]
        public bool _empty_tr = false;
        [NotMapped]
        public int? new_position { get; set; }
        [NotMapped]
        public bool _start_drag = false;
        [NotMapped]
        public int? _drag_position
        {
            get
            {
                if (_start_drag)
                    return new_position;
                else
                    return product_postition;
            }
        }
        public int? product_postition { get; set; } = 0;
        public int? from_delivery_period { get; set; } = 1;
        public int? to_delivery_period { get; set; } = 3;
        public int? delivery_period_type_id { get; set; }

        //public int? order_id { get; set; } = null;
        public double? ruble_discount { get; set; }
        public int? percent_discount { get; set; }
        public int? supplier_delivery_period { get; set; }
        public int? order_check_id { get; set; } = 0;
        public double? supplier_price { get; set; }
        [ForeignKey("product_id")]
        public product? product { get; set; }
        [ForeignKey("order_check_id")]
        public virtual order_check? order_check { get; set; }
        //[ForeignKey("order_id")]
        //public order? order { get; set; }

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
                    return Math.Round(Convert.ToDouble(product_price), 2);
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
        public int? status_id { get; set; }
        public double? _delivery_products_total_price
        {
            get
            {
                return product_to_orders?.Select(p => p.supplier_price).Sum();
            }
        }

        [ForeignKey("status_id")]
        public supplier_order_status? supplier_order_status { get; set; }

        [ForeignKey("user_id")]
        public user? user { get; set; }

        [ForeignKey("supplier_id")]
        public client? supplier { get; set; } = new();
        public List<product_to_order>? product_to_orders { get; set; } = new();
    }
    public class supplier_order_status
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
    public class delivery
    {
        [Key]
        public int id { get; set; }
        public DateTime? shipment_date { get; set; }
        public int? delivery_type_id { get; set; } = 1;
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
                return Math.Round(Convert.ToDouble(price), 2);
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
        public int? retail_id { get; set; }
        public DateTime? date { get; set; }
        [NotMapped]
        public bool? is_new { get; set; }
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
