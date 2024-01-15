using System.Threading.Tasks;
using SalesProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index()
        {
            if(_context.sales_fact == null)
            {
                return Problem("Entity set 'AppDbContext.sales_fact'  is null.");
            }

            var selectquery = "SELECT sf.invoice_id, sf.location_id, sl.branch, sf.product_id,sp.product_category, " +
                            "sf.date_id, sd.datetime_of_purchase,sf.customer_id, sc.customer_type,sc.gender," +
                            "sf.unit_price, sf.quantity_sold, sf.total_price_of_goods_sold, sf.tax_5_percent, sf.total_sales_after_taxes, " +
                            "sf.payment_type FROM sales_fact sf " +
                            "JOIN supermarket_location sl On sf.location_id= sl.location_id " +
                            "JOIN supermarket_product sp On sf.product_id = sp.product_id " +
                            "JOIN supermarket_date sd On sf.date_id = sd.date_id " +
                            "JOIN supermarket_customer sc On sf.customer_id= sc.customer_id";

            var salesModel = await _context.sales_fact.FromSqlRaw(selectquery).ToListAsync();
          
            return View(salesModel);
                        
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
            //IEnumerable<SelectListItem> ItemsList = from item in products
            //                                        select new SelectListItem
            //                                        {
            //                                            Value = Convert.ToString(item.product_id),
            //                                            Text = item.product_category
            //                                        };
            ViewBag.Products = products;

            List<LocationModel> locations = _context.supermarket_location.ToList();
            IEnumerable<SelectListItem> ItemsList = from item in locations
                                                    select new SelectListItem
                                                    {
                                                        Value = Convert.ToString(item.location_id),
                                                        Text =  item.branch + " - " + item.city
                                                    };
            ViewBag.Locations = ItemsList;
            List<string> payment_type = new List<string>()
            {
                "Credit Card",
                "Cash",
                "EWallet"
            };
            ViewBag.PaymentType = payment_type;
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        
        [HttpPost("Create" )]
        //[ValidateAntiForgeryToken]
      //  [ResponseType(typeof(IEnumerable<СompositeType>))]
        public async Task<IActionResult> Create([FromForm]SalesModel salesModel)
        {
            if (ModelState.IsValid)
            {
                salesModel.total_price_of_goods_sold = salesModel.unit_price * salesModel.quantity_sold;
                salesModel.tax_5_percent = 0.05 * salesModel.total_price_of_goods_sold;
                _context.Add(salesModel);
                await _context.SaveChangesAsync();
                ViewBag.Message = "Data Insert Successfully";
            }
            return View();
        }

        // GET: Sales/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.sales_fact == null)
            {
                return NotFound();
            }

            var salesModel = await _context.sales_fact.FindAsync(id);
            if (salesModel == null)
            {
                return NotFound();
            }
            return View(salesModel);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [FromForm]SalesModel salesModel)
        {
            if (id != salesModel.invoice_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    salesModel.total_price_of_goods_sold = salesModel.unit_price * salesModel.quantity_sold;
                    salesModel.tax_5_percent = 0.05 * salesModel.total_price_of_goods_sold;

                    _context.Update(salesModel);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.sales_fact == null)
            {
                return Problem("Entity set 'AppDbContext.sales_fact'  is null.");
            }
            var salesModel = await _context.sales_fact.FindAsync(id);
            if (salesModel != null)
            {
                _context.sales_fact.Remove(salesModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesModelExists(string id)
        {
            return (_context.sales_fact?.Any(e => e.invoice_id == id)).GetValueOrDefault();
        }
    }
    }
