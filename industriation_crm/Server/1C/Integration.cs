﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using industriation_crm.Shared.Models;
using ITworks.Brom;
using ITworks.Brom.Types;

namespace industriation_crm.Server._1C
{
    public class Integration1C
    {
        dynamic клиент = null;
        public Integration1C()
        {
            клиент = new БромКлиент(@"
	Публикация	= http://46.29.118.52/buh;
	Пользователь	= buhgalter;
	Пароль		= LhPBBqUnk
");
        }
        public void AddNewSupplierOrder(_1CSupplierOrder _1CSupplierOrder)
        {
            Запрос запрос = клиент.СоздатьЗапрос($"ВЫБРАТЬ * ИЗ Документ.счетНаОплатуПоставщика");
            ТаблицаЗначений тест = (ТаблицаЗначений)запрос.Выполнить();


            dynamic счетНаОплатуПоставщику = клиент.Документы.СчетНаОплатуПоставщика.СоздатьДокумент();
            dynamic контрагент = AddContragent(_1CSupplierOrder?.contragent!);
            счетНаОплатуПоставщику.Контрагент = контрагент.Ссылка;
            счетНаОплатуПоставщику.Номер = _1CSupplierOrder?.id;
            List<_1CProduct> products = AddProducts(_1CSupplierOrder?.products!);

            foreach (var p in products)
            {
                dynamic товар = счетНаОплатуПоставщику.Товары.Добавить();
                товар.Номенклатура = p.reference1c;
                товар.Количество = p.count;

                товар.Цена = p.price;
                товар.Сумма = p.summ;
                товар.СтавкаНДС = клиент.Перечисления.СтавкиНДС.НДС20;
            }

            счетНаОплатуПоставщику.Записать(РежимЗаписиДокумента.Проведение);
            AddSupplierPayment(контрагент, счетНаОплатуПоставщику);
        }
        private void AddSupplierPayment(dynamic контрагент, dynamic счетНаОплатуПоставщику)
        {
            double? price = Convert.ToDouble(счетНаОплатуПоставщику.СуммаДокумента);
            Запрос запрос = клиент.СоздатьЗапрос($"ВЫБРАТЬ * ИЗ Документ.ПлатежноеПоручение");
            ТаблицаЗначений тест = (ТаблицаЗначений)запрос.Выполнить();
            double? nds = (price / 1.2) / 100 * 20;

            dynamic платежноеПоручение = клиент.Документы.ПлатежноеПоручение.СоздатьДокумент();

            платежноеПоручение.Контрагент = контрагент.Ссылка;
            платежноеПоручение.СчетКонтрагента = контрагент.ОсновнойБанковскийСчет;
            платежноеПоручение.ДокументОснование = счетНаОплатуПоставщику.Ссылка;
            платежноеПоручение.СуммаДокумента = счетНаОплатуПоставщику.СуммаДокумента;
            платежноеПоручение.ВидОперации = клиент.Перечисления.ВидыОперацийСписаниеДенежныхСредств.ОплатаПоставщику;

            запрос = клиент.СоздатьЗапрос($"ВЫБРАТЬ * ИЗ Справочник.Организации");
            ТаблицаЗначений организации = (ТаблицаЗначений)запрос.Выполнить();
            dynamic организация = организации.FirstOrDefault();

            платежноеПоручение.Организация = организация.Ссылка;
            платежноеПоручение.СчетОрганизации = организация.ОсновнойБанковскийСчет;
            платежноеПоручение.ИННПлательщика = организация.ИНН;
            платежноеПоручение.ИННПолучателя = контрагент.ИНН;

            платежноеПоручение.ТекстПлательщика = организация.НаименованиеПолное;
            платежноеПоручение.ТекстПолучателя = контрагент.НаименованиеПолное;
            платежноеПоручение.СтатьяДвиженияДенежныхСредств = клиент.Справочники.СтатьиДвиженияДенежныхСредств.ОплатаПоставщику;
            платежноеПоручение.НазначениеПлатежа = $"Оплата по счету";
            платежноеПоручение.СтавкаНДС = клиент.Перечисления.СтавкиНДС.НДС20;
            платежноеПоручение.СуммаНДС = nds;
            платежноеПоручение.Записать(РежимЗаписиДокумента.Проведение);
        }
        public void AddNewOrderPay(_1COrderPay orderPay)
        {

            Запрос запрос = клиент.СоздатьЗапрос($"ВЫБРАТЬ * ИЗ Справочник.Пользователи ГДЕ Наименование=\"Бухгалтер\"");
            ТаблицаЗначений ответственные = (ТаблицаЗначений)запрос.Выполнить();
            dynamic ответственный = ответственные.FirstOrDefault();
            dynamic ответственныйСсылка = ответственный.Ссылка;

            запрос = клиент.СоздатьЗапрос($"ВЫБРАТЬ * ИЗ Справочник.Организации");
            ТаблицаЗначений организации = (ТаблицаЗначений)запрос.Выполнить();
            dynamic организация = организации.FirstOrDefault();
            dynamic ссылкаНаОрганизацию = организация.Ссылка;

            dynamic счетНаОплатуПокупателю = клиент.Документы.СчетНаОплатуПокупателю.СоздатьДокумент();
            счетНаОплатуПокупателю.Номер = orderPay.id;
            счетНаОплатуПокупателю.Дата = DateTime.Today;

            
            счетНаОплатуПокупателю.Организация = ссылкаНаОрганизацию;
            счетНаОплатуПокупателю.ОрганизацияПолучатель = ссылкаНаОрганизацию;
            счетНаОплатуПокупателю.Ответственный = ответственныйСсылка;

            dynamic СсылкаНаКонтрагента = AddContragent(orderPay.contragent);
            List<_1CProduct> products = AddProducts(orderPay.products);

            счетНаОплатуПокупателю.Контрагент = СсылкаНаКонтрагента;
            // Заполняем табличную часть "Товары"
            foreach (var p in products)
            {
                dynamic товар = счетНаОплатуПокупателю.Товары.Добавить();
                товар.Номенклатура = p.reference1c;
                товар.Количество = p.count;

                товар.Цена = p.price;
                товар.Сумма = p.summ;
                товар.СтавкаНДС = клиент.Перечисления.СтавкиНДС.НДС20;
            }
            счетНаОплатуПокупателю.СуммаВключаетНДС = true;
            счетНаОплатуПокупателю.Записать(РежимЗаписиДокумента.Проведение);

        }
        private dynamic AddContragent(_1CContragent contragent)
        {
            Запрос запрос = клиент.СоздатьЗапрос($"ВЫБРАТЬ * ИЗ Справочник.Контрагенты КАК Контрагенты ГДЕ Контрагенты.ИНН = \"{contragent.inn}\"");

            ТаблицаЗначений контрагент = (ТаблицаЗначений)запрос.Выполнить();
            dynamic контрагентСсылка = null;
            if (контрагент.Count != 0)
            {
                foreach (dynamic r in контрагент)
                {
                    контрагентСсылка = r.Ссылка;
                }
            }
            else
            {
                dynamic новыйКонтрагент = клиент.Справочники.Контрагенты.СоздатьЭлемент();
                новыйКонтрагент.ЮридическоеФизическоеЛицо = клиент.Перечисления.ЮридическоеФизическоеЛицо.ЮридическоеЛицо;
                новыйКонтрагент.Наименование = contragent.name_in_programm;
                новыйКонтрагент.ИНН = contragent.inn;
                новыйКонтрагент.РегистрационныйНомер = contragent.ogrn;
                новыйКонтрагент.КПП = contragent.kpp;
                новыйКонтрагент.Записать();

                //добавление расчетного счета
                if (contragent.bik != null && contragent.ras_schet != null)
                {
                    dynamic Банк = клиент.Справочники.Банки.НайтиПоКоду(contragent.bik);
                    dynamic банковскийСчет = клиент.Справочники.БанковскиеСчета.СоздатьЭлемент();
                    банковскийСчет.Банк = Банк.Ссылка;
                    банковскийСчет.НомерСчета = contragent.ras_schet;
                    банковскийСчет.Владелец = новыйКонтрагент.Ссылка;
                    банковскийСчет.Записать();
                    новыйКонтрагент.ОсновнойБанковскийСчет = банковскийСчет.Ссылка;
                    новыйКонтрагент.Записать();
                }


                контрагентСсылка = новыйКонтрагент.Ссылка;
            }
            return контрагентСсылка;
        }

        private List<_1CProduct> AddProducts(List<_1CProduct> products)
        {
            Запрос запрос = клиент.СоздатьЗапрос($"ВЫБРАТЬ * ИЗ Справочник.КлассификаторЕдиницИзмерения КАК Единица ГДЕ Единица.Наименование=\"шт\"");
            ТаблицаЗначений таблицаЕдИзмерения = (ТаблицаЗначений)запрос.Выполнить();
            dynamic едИзмерения = таблицаЕдИзмерения.FirstOrDefault();
            dynamic едИзмеренияСсылка = едИзмерения.Ссылка;

            запрос = клиент.СоздатьЗапрос($"ВЫБРАТЬ * ИЗ Справочник.ВидыНоменклатуры  КАК ВидыНоменклатуры ГДЕ ВидыНоменклатуры.Наименование=\"Товары\"");
            ТаблицаЗначений таблицаВидНоменклатурыИзмерения = (ТаблицаЗначений)запрос.Выполнить();
            dynamic видНоменклатуры = таблицаВидНоменклатурыИзмерения.FirstOrDefault();
            dynamic видНоменклатурыСсылка = видНоменклатуры.Ссылка;

            List<dynamic> ссылкиНаТовары = new List<dynamic>();
            foreach (var p in products)
            {
                запрос = клиент.СоздатьЗапрос($"ВЫБРАТЬ * ИЗ Справочник.Номенклатура КАК Номенклатура ГДЕ Номенклатура.Артикул = \"{p.article}\"");
                ТаблицаЗначений товар = (ТаблицаЗначений)запрос.Выполнить();
                if (товар.Count != 0)
                {
                    dynamic t = товар.FirstOrDefault();
                    p.reference1c = t.Ссылка;
                }
                else
                {
                    Console.WriteLine("Товар не найден, создание нового");
                    dynamic новыйТовар = клиент.Справочники.Номенклатура.СоздатьЭлемент();
                    новыйТовар.Наименование = p.name;
                    новыйТовар.Артикул = p.article;
                    новыйТовар.ЕдиницаИзмерения = едИзмеренияСсылка;
                    новыйТовар.ВидСтавкиНДС = клиент.Перечисления.ВидыСтавокНДС.Общая;
                    новыйТовар.ВидНоменклатуры = видНоменклатурыСсылка;
                    новыйТовар.Записать();
                    dynamic товарСсылка = новыйТовар.Ссылка;
                    p.reference1c = товарСсылка;
                }
            }
            return products;
        }
    }
}
