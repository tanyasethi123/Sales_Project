using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Mvc;
using SalesProject.Model;
using X.PagedList;

namespace SalesProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : Controller
    {
        private readonly ILogger<SalesController> _logger;
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context, ILogger<SalesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //GET: Sales
        [HttpGet]
        public IActionResult Index(string? sortOrder,string? currentFilter, string? searchString, int? page)
        {
            if (_context.sales_fact == null)
            {
                return Problem("Entity set 'AppDbContext.sales_fact'  is null.");
            }

            //string sortOrder = "inv_desc";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.InvoiceSortParam = String.IsNullOrEmpty(sortOrder)? "inv_desc" : "";
            ViewBag.CustomerSortParm = sortOrder == "Customer Type" ? "cust_desc" : "Customer Type";
            ViewBag.GenderSortParm = sortOrder == "Gender" ? "gen_desc" : "Gender";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.ProductSortParm = sortOrder == "Product Category" ? "prod_desc" : "Product Category";
            ViewBag.BranchSortParm = sortOrder == "Branch" ? "branch_desc" : "Branch";

           
            var selquery = (from sf in _context.sales_fact
                            join sl in _context.supermarket_location on sf.location_id equals sl.location_id
                            join sp in _context.supermarket_product on sf.product_id equals sp.product_id
                            join sd in _context.supermarket_date on sf.date_id equals sd.date_id
                            join sc in _context.supermarket_customer on sf.customer_id equals sc.customer_id
                           
                            select new SalesViewModel
                            {
                                invoice_id = sf.invoice_id,
                                location_id = sf.location_id,
                                branch = sl.branch,
                                product_id = sf.product_id,
                                product_category = sp.product_category,
                                date_id = sf.date_id,
                                datetime_of_purchase = sd.datetime_of_purchase,
                                customer_id = sf.customer_id,
                                customer_type = sc.customer_type,
                                gender = sc.gender,
                                unit_price = sf.unit_price,
                                quantity_sold = sf.quantity_sold,
                                total_price_of_goods_sold = sf.total_price_of_goods_sold,
                                tax_5_percent = sf.tax_5_percent,
                                total_sales_after_taxes = sf.total_sales_after_taxes,
                                payment_type = sf.payment_type

                            });
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                selquery = selquery.Where(s => s.invoice_id.ToLower().Contains(searchString.ToLower()));
            }

            switch (sortOrder)
            {
                case "inv_desc":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderByDescending(s => s.invoice_id);
                    break;
                case "Customer Type":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderBy(s => s.customer_type);
                    break;
                case "cust_desc":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderByDescending(s => s.customer_type);
                    break;
                case "Gender":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderBy(s => s.gender);
                    break;
                case "gen_desc":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderByDescending(s => s.gender);
                    break;
                case "date_desc":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderByDescending(s => s.datetime_of_purchase);
                    break;
                case "Date":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderBy(s => s.datetime_of_purchase);
                    break;
                case "prod_desc":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderByDescending(s => s.product_category);
                    break;
                case "Product Category":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderBy(s => s.product_category);
                    break;
                case "branch_desc":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderByDescending(s => s.branch);
                    break;
                case "Branch":
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderBy(s => s.branch);
                    break;
                default:
                    selquery = (IQueryable<SalesViewModel>)selquery.OrderBy(s => s.invoice_id);
                    break;
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(selquery.ToPagedList(pageNumber, pageSize));
                        
        }

        // GET: Sales/Details/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.sales_fact == null)
            {
                return NotFound();
            }

            var salesModel = await _context.sales_fact
                .FirstOrDefaultAsync(m => m.invoice_id == id);
            if (salesModel == null)
            {
                return NotFound();
            }

            return View(salesModel);
        }

        // GET: Sales/Create
        [HttpGet("Create")]
        public ActionResult Create()
        {
            setData();
            return View();
        }

        // POST: Sales/Create
        [HttpPost("Create" )]
        public  IActionResult Create([FromForm]SalesViewModel salesModel)
        {
            if (ModelState.IsValid)
            {
                var custObj = _context.supermarket_customer.FirstOrDefault(m => m.customer_type == salesModel.customer_type && m.gender == salesModel.gender);

                salesModel.customer_id = custObj?.customer_id?.ToString();

                var dateObj = _context.supermarket_date.FirstOrDefault(m => m.datetime_of_purchase == salesModel.datetime_of_purchase);
                if(dateObj != null)
                {
                    salesModel.date_id = dateObj.date_id;
                }
                else
                {
                    //Create a new date
                   _context.Database.ExecuteSqlRaw("INSERT INTO supermarket_date(datetime_of_purchase) VALUES ({0}); ", salesModel.datetime_of_purchase);
                    var newDateObj = _context.supermarket_date.FirstOrDefault(m => m.datetime_of_purchase == salesModel.datetime_of_purchase);
                    salesModel.date_id = newDateObj.date_id;
                    //   salesModel.date_id = newDateObj?.date_id?.ToString();

                }
                
                salesModel.total_price_of_goods_sold = salesModel.unit_price * salesModel.quantity_sold;
                salesModel.tax_5_percent = 0.05 * salesModel.total_price_of_goods_sold;

                var insertQuery = "INSERT INTO sales_fact(invoice_id, location_id, product_id, date_id, customer_id, unit_price, " +
                                    "quantity_sold, total_price_of_goods_sold, tax_5_percent, total_sales_after_taxes, payment_type) " +
                                    "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}) ";
                _context.Database.ExecuteSqlRaw(insertQuery,
                                  salesModel.invoice_id!, salesModel.location_id!, salesModel.product_id!, salesModel.date_id!,
                                                 salesModel.customer_id!, salesModel.unit_price, salesModel.quantity_sold, salesModel.total_price_of_goods_sold,
                                                 salesModel.tax_5_percent, salesModel.total_sales_after_taxes, salesModel.payment_type!);
                ViewBag.Message = "Data Insert Successfully";
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: Sales/Edit/5
        [HttpGet("Edit/{id}")]
        public IActionResult Edit(string id)
        {
            setData();
            if (id == null || _context.sales_fact == null)
            {
                return NotFound();
            }
            var selectQueryforEdit = (from sf in _context.sales_fact
                               join sl in _context.supermarket_location on sf.location_id equals sl.location_id
                               join sp in _context.supermarket_product on sf.product_id equals sp.product_id
                               join sd in _context.supermarket_date on sf.date_id equals sd.date_id
                               join sc in _context.supermarket_customer on sf.customer_id equals sc.customer_id
                               where sf.invoice_id == id
                               select new SalesViewModel
                               {
                                   invoice_id = sf.invoice_id,
                                   location_id = sf.location_id,
                                   branch = sl.branch,
                                   product_id = sf.product_id,
                                   product_category = sp.product_category,
                                   date_id = sf.date_id,
                                   datetime_of_purchase = sd.datetime_of_purchase,
                                   customer_id = sf.customer_id,
                                   customer_type = sc.customer_type,
                                   gender = sc.gender,
                                   unit_price = sf.unit_price,
                                   quantity_sold = sf.quantity_sold,
                                   total_price_of_goods_sold = sf.total_price_of_goods_sold,
                                   tax_5_percent = sf.tax_5_percent,
                                   total_sales_after_taxes = sf.total_sales_after_taxes,
                                   payment_type = sf.payment_type

                               }).First();

           // var salesModel =  _context.sales_fact.FromSqlRaw(selectquery,id).First();

            if (selectQueryforEdit == null)
            {
                return NotFound();
            }
            return View(selectQueryforEdit);
        }

        // POST: Sales/Edit/5
        [HttpPost("Edit/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [FromForm]SalesViewModel salesModel)
        {
            if (id != salesModel.invoice_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var custObj = _context.supermarket_customer.FirstOrDefault(m => m.customer_type == salesModel.customer_type && m.gender == salesModel.gender);

                    salesModel.customer_id = custObj?.customer_id?.ToString();

                    var dateObj = _context.supermarket_date.FirstOrDefault(m => m.datetime_of_purchase == salesModel.datetime_of_purchase);
                    if (dateObj != null)
                    {
                        salesModel.date_id = dateObj.date_id;
                    }
                    else
                    {
                        //Create a new date
                        _context.Database.ExecuteSqlRaw("INSERT INTO supermarket_date(datetime_of_purchase) VALUES ({0}); ", salesModel.datetime_of_purchase!);
                        var newDateObj = _context.supermarket_date.FirstOrDefault(m => m.datetime_of_purchase == salesModel.datetime_of_purchase);
                        salesModel.date_id = newDateObj?.date_id;

                    }
                    salesModel.total_price_of_goods_sold = salesModel.unit_price * salesModel.quantity_sold;
                    salesModel.tax_5_percent = 0.05 * salesModel.total_price_of_goods_sold;
                    var updateQuery = "UPDATE sales_fact SET  location_id={0}, product_id={1}, date_id={2}, customer_id={3}, unit_price={4}, " +
                                  "quantity_sold={5}, total_price_of_goods_sold={6}, tax_5_percent={7}, total_sales_after_taxes={8}, payment_type={9} " +
                                  " WHERE invoice_id={10} ";
                    _context.Database.ExecuteSqlRaw(updateQuery,
                                                    salesModel.location_id!, salesModel.product_id!, salesModel.date_id!,
                                                    salesModel.customer_id!, salesModel.unit_price, salesModel.quantity_sold, salesModel.total_price_of_goods_sold,
                                                    salesModel.tax_5_percent, salesModel.total_sales_after_taxes, salesModel.payment_type!, salesModel.invoice_id!);

                  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesModelExists(salesModel.invoice_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(salesModel);
        }

        // GET: Sales/Delete/5
        [HttpGet("Delete")]
        public ActionResult Delete()
        {
            return View();
        }

        // POST: Sales/Delete/5
        [HttpPost("Delete")]
        // [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            if (_context.sales_fact == null)
            {
                return Problem("Entity set 'AppDbContext.sales_fact'  is null.");
            }
            var selectQueryForDelete = (from sf in _context.sales_fact
                                        join sl in _context.supermarket_location on sf.location_id equals sl.location_id
                                        join sp in _context.supermarket_product on sf.product_id equals sp.product_id
                                        join sd in _context.supermarket_date on sf.date_id equals sd.date_id
                                        join sc in _context.supermarket_customer on sf.customer_id equals sc.customer_id
                                        where sf.invoice_id == id
                                        select new SalesViewModel
                                        {
                                            invoice_id = sf.invoice_id,
                                            location_id = sf.location_id,
                                            branch = sl.branch,
                                            product_id = sf.product_id,
                                            product_category = sp.product_category,
                                            date_id = sf.date_id,
                                            datetime_of_purchase = sd.datetime_of_purchase,
                                            customer_id = sf.customer_id,
                                            customer_type = sc.customer_type,
                                            gender = sc.gender,
                                            unit_price = sf.unit_price,
                                            quantity_sold = sf.quantity_sold,
                                            total_price_of_goods_sold = sf.total_price_of_goods_sold,
                                            tax_5_percent = sf.tax_5_percent,
                                            total_sales_after_taxes = sf.total_sales_after_taxes,
                                            payment_type = sf.payment_type

                                        }).First();

        //    var salesModel = _context.sales_fact.FromSqlRaw(selectQueryForDelete, id).First();
            if (selectQueryForDelete != null)
            {
                var deleteQuery = "DELETE FROM sales_fact WHERE invoice_id={0}";
                _context.Database.ExecuteSqlRaw(deleteQuery,id);
            }
                
            return RedirectToAction(nameof(Index));
        }

        private bool SalesModelExists(string id)
        {
            return (_context.sales_fact?.Any(e => e.invoice_id == id)).GetValueOrDefault();
        }

        private void setData()
        {
            List<string> gender = new List<string>()
            {
                "Male",
                "Female"
            };
            ViewBag.Gender = gender;

            List<string> customer_type = new List<string>()
            {
                "Member",
                "Non-Member"
            };
            ViewBag.CustomerType = customer_type;

            List<ProductModel> products = _context.supermarket_product.ToList();
            ViewBag.Products = products;

            List<LocationModel> locations = _context.supermarket_location.ToList();
            IEnumerable<SelectListItem> ItemsList = from item in locations
                                                    select new SelectListItem
                                                    {
                                                        Value = Convert.ToString(item.location_id),
                                                        Text = item.branch + " - " + item.city
                                                    };
            ViewBag.Locations = ItemsList;
            List<string> payment_type = new List<string>()
            {
                "Credit Card",
                "Cash",
                "EWallet"
            };
            ViewBag.PaymentType = payment_type;

        }
    }
    }
