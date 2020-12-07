using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using course.Common;
using course.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace course.Parser
{
    public class SiteParser
    {
        /// <summary>
        /// Собираем информацию о товарах на главной
        /// </summary>
        /// <returns>Коллекция с товарами с главной страницей</returns>
        public async Task<Collection<Product>> ParseHomePageProducts()
        {
            try
            {
                Collection<Product> avitoProducts = new Collection<Product>();

                /// Парсим домашнюю страницу
                var homePageDocument = await DocumentParser.GetDocument(@"https://www.avito.ru/tomsk/noutbuki");
                /// Ищем карточки товаров через LINQ запрос
                var productCards = homePageDocument.All.Filter(AvitoSelector.ProductCardSelector).ToCollection<IElement>();

                ///// Проходим по каждой карточке и заходим на страницу товара
                foreach (var productCard in productCards)
                {
                    /// Сначала парсим инфу о товаре из карточки
                    var firstProductInfo = ParseProductDataFromCard(productCard);
                    /// Затем парсим информацию с его детальной страницы
                    Thread.Sleep(2000);
                    var resutlProductInfo = await ParseProductPage(firstProductInfo);
                    /// Ну и добавляем в массив
                    avitoProducts.Add(resutlProductInfo);
                }

                return avitoProducts;
            } catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Сбор информации о товаре из карточки товара
        /// </summary>
        /// <param name="productCardElement">Элемент карточки товара</param>
        /// <returns>Данные товара</returns>
        private Product ParseProductDataFromCard(IElement productCardElement)
        {
            /// Создаем объект для хранения инфы о товаре
            var product = new Product
            {
                /// Парсим название по селекторам
                Name = productCardElement.QuerySelector(AvitoSelector.ProductCardTitleSelector).TextContent,
                /// Аналогично цену с валютой
                Price = int.Parse(OfferParser.GetOfferData(productCardElement.QuerySelector(AvitoSelector.ProductCardOffersSelector))?.Price),
                /// Парсим ссылку на страницу
                Link = productCardElement.QuerySelector<IHtmlAnchorElement>(AvitoSelector.ProductCardLinkSelector).Href
            };

            return product;
        }

        /// <summary>
        /// Сбор информации о товаре со страницы товара
        /// </summary>
        /// <param name="avitoProduct">Данные товара</param>
        /// <returns>Новый объект с дополненными данными товара</returns>
        private async Task<Product> ParseProductPage(Product avitoProduct)
        {
            var newProduct = new Product();

            /// Парсим страницу товара
            var productPage = await DocumentParser.GetDocument(avitoProduct.Link);

            /// Копируем имеющиеся данные в новый объект
            newProduct.Name = avitoProduct.Name;
            newProduct.Price = avitoProduct.Price;
            newProduct.Link = avitoProduct.Link;
            /// Добавляем новые со страницы товара
            newProduct.Visitors = productPage.QuerySelector(AvitoSelector.ProductPageVisitors)?.TextContent.Trim();
            newProduct.Number = productPage.QuerySelector(AvitoSelector.ProductPageNumber)?.TextContent;
            newProduct.Description = productPage.QuerySelector(AvitoSelector.ProductPageDescription)?.TextContent;

            var linkBlock = productPage.QuerySelector<IHtmlAnchorElement>(AvitoSelector.ProductSellerNameLink);
            if (linkBlock != null)
            {
                var productSeller = new Seller
                {
                    Name = linkBlock.TextContent.Trim(),
                    Link = linkBlock.Href
                };

                newProduct.Seller = productSeller;
            }
            return newProduct;

        }
    }
}
