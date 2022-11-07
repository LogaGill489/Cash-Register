using System;
using System.Media;
using System.Threading;
using System.Windows.Forms;

/*
 * Mario Cash Register Program
 * Logan Gillett
 * Mr T.
 * 11.6.22
 * ICS3U
 */

namespace Cash_Register
{

    public partial class Form1 : Form
    {
        //prices and variables for each item
        double dash;
        double dashprice = 25;
        double poison;
        double poisonprice = 30;
        double golden;
        double goldenprice = 50;

        //output of costs and total costs
        double subtotal;
        double tax;
        double taxpercent = 0.13;
        double total;

        //variables for the users payment
        double coins;
        double stars;
        double customertotal;
        double customerchange;

        //displayable variables for inputs
        double starsubtotal;
        double coinsubtotal;

        double startax;
        double cointax;

        double startotal;
        double cointotal;

        double customerstars;
        double customercoins;

        //displaces the amount of coins per star in a variable
        int starsconversion = 100;

        //integer to calculate total customers
        int customer = 0;

        SoundPlayer player = new SoundPlayer(Properties.Resources.receipt);
        SoundPlayer playercut = new SoundPlayer(Properties.Resources.receiptcut);
        public Form1()
        {
            InitializeComponent();

            //automatically sets each value to 0
            dashInput.Text = "0";
            poisonInput.Text = "0";
            goldenInput.Text = "0";
        }

        private void totalButton_Click(object sender, EventArgs e)
        {
            try
            {
                //give each variable a value
                dash = Convert.ToDouble(dashInput.Text);
                poison = Convert.ToDouble(poisonInput.Text);
                golden = Convert.ToDouble(goldenInput.Text);

                //calculating necessary doubles for display
                subtotal = (dash * dashprice) + (poison * poisonprice) + (golden * goldenprice);
                starsubtotal = Convert.ToInt16(subtotal) / starsconversion;
                coinsubtotal = subtotal % starsconversion;

                tax = subtotal * taxpercent;
                startax = Convert.ToInt16(tax) / starsconversion;
                cointax = Convert.ToInt16(tax) % starsconversion;

                total = subtotal + tax;
                startotal = Convert.ToInt16(total) / starsconversion;
                cointotal = Convert.ToInt16(total) % starsconversion;

                //detects if the amount being bought is above the acceptable limit and gives an error message if true
                if (dash > 200 || dash < 0 || poison > 200 || poison < 0 || golden > 200 || golden < 0 || dash == 0 && poison == 0 && golden == 0)
                {
                    receiptLabel.Text = $"The Item Amount Inserted is Incompatable.";
                    receiptLabel.Text += $"\n\nPlease Change the Amount of Product.";
                }
                else
                {
                    //subtotal display
                    starsSubtotalOutput.Text = $"{starsubtotal}";
                    coinsSubtotalOutput.Text = $"{coinsubtotal}";

                    //tax display
                    starsTaxOutput.Text = $"{startax}";
                    coinsTaxOutput.Text = $"{cointax}";

                    //total display
                    starsTotalOutput.Text = $"{startotal}";
                    coinsTotalOutput.Text = $"{cointotal}";

                    //turns the "calculate change" button on
                    changeButton.Enabled = true;
                    printButton.Enabled = false;

                    //shows stars and coins upon button activation
                    subtotalStarPicture.Visible = true;
                    taxStarPicture.Visible = true;
                    totalStarPicture.Visible = true;
                    subtotalCoinPicture.Visible = true;
                    taxCoinPicture.Visible = true;
                    totalCoinPicture.Visible = true;

                    //resets recipt text if all requirements are meet to procede with the purchase
                    receiptLabel.Text = "";
                }
            }
            catch
            {
                receiptLabel.Text = "An Error has Occured.";
                receiptLabel.Text += "\n\nPlease Fill Each Box With a Value of Numbers.";
            }
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            try
            {
                //sets variables for inputs
                coins = Convert.ToDouble(coinPaymentInput.Text);
                stars = Convert.ToDouble(starPaymentInput.Text);

                customertotal = coins + (stars * 100);
                customerchange = customertotal - Convert.ToInt16(total);

                //detects if the customers total is above the acceptable limit and gives an error message if true
                if (stars > 200 || coins > 9900)
                {
                    receiptLabel.Text = "Too Many Stars or Coins Have Been Inserted.";
                    receiptLabel.Text += "\n\nPlease Insert a Smaller Amount.";
                }
                //checks to confirm that the change is above 0 so that the customer can't buy unless they input enough currency
                else if (customerchange >= 0)
                {
                    customerstars = Convert.ToInt16(customerchange) / 100;
                    customercoins = Convert.ToInt16(customerchange) % 100;

                    starsChangeOutput.Text = $"{customerstars}";
                    coinsChangeOutput.Text = $"{customercoins}";

                    //resets recipt text
                    receiptLabel.Text = "";

                    //shows stars and coins if there is enough payment
                    changeStarPicture.Visible = true;
                    changeCoinPicture.Visible = true;

                    //print button becomes enable if critera is met
                    printButton.Enabled = true;
                }
                else
                {
                    //displays that there is not enough payment
                    receiptLabel.Text = "Not Enough Payment Given.";
                    receiptLabel.Text += "\n\nPlease Provide More Money!";

                    //stops other buttons from working
                    printButton.Enabled = false;

                    //resets the payment
                    changeStarPicture.Visible = false;
                    changeCoinPicture.Visible = false;
                    starsChangeOutput.Text = "";
                    coinsChangeOutput.Text = "";
                }
            }
            catch
            {
                receiptLabel.Text = $"An Error has Occured.";
                receiptLabel.Text += $"\n\nPlease Fill Each Box With a Value of Numbers.";
            }
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            int receiptSleep = 1000;

            //stops the reuse of the calculate, change or print recipt button
            totalButton.Enabled = false;
            changeButton.Enabled = false;
            printButton.Enabled = false;

            //temporary increment for customer counting
            customer = customer + 1;

            //from here to the bottom of the private void is the printing animation of the recipt
            //displays company name, order number, current date and creates a gap for if an item needs to be hidden
            receiptLabel.Text = "\n\n       Mario Party Sales Inc.";
            player.Play();
            Refresh();
            Thread.Sleep(receiptSleep);

            receiptLabel.Text += $"\n\n  Order Number {customer}";
            player.Play();
            Refresh();
            Thread.Sleep(receiptSleep);

            receiptLabel.Text += $"\n  {DateTime.Now.ToString("MMMM dd, yyyy")}";
            receiptLabel.Text += $"\n";
            player.Play();
            Refresh();
            Thread.Sleep(receiptSleep);

            //displays items purchased and hides any items that have a value of 0
            if (dash > 0)
            {
                receiptLabel.Text += $"\n  Dash Mushrooms   x{dash} @ {dashprice} coins";
                player.Play();
                Refresh();
                Thread.Sleep(receiptSleep);
            }

            if (poison > 0)
            {
                receiptLabel.Text += $"\n  Poison Mushrooms x{poison} @ {poisonprice} coins";
                player.Play();
                Refresh();
                Thread.Sleep(receiptSleep);
            }

            if (golden > 0)
            {
                receiptLabel.Text += $"\n  Golden Mushrooms x{golden} @ {goldenprice} coins";
                player.Play();
                Refresh();
                Thread.Sleep(receiptSleep);
            }

            //changes pricing to a more displayable format
            subtotal = subtotal / 100;
            tax = tax / 100;
            total = total / 100;
            customertotal = customertotal / 100;
            customerchange = customerchange / 100;

            //diplays subtotal, tax and total
            receiptLabel.Text += $"\n\n  Subtotal        {subtotal.ToString("C")}";
            player.Play();
            Refresh();
            Thread.Sleep(receiptSleep);

            receiptLabel.Text += $"\n  Tax             {tax.ToString("C")}";
            player.Play();
            Refresh();
            Thread.Sleep(receiptSleep);

            receiptLabel.Text += $"\n  Total           {total.ToString("C")}";
            player.Play();
            Refresh();
            Thread.Sleep(receiptSleep);


            //displays tendered and change due
            receiptLabel.Text += $"\n\n  Tendered        {customertotal.ToString("C")}";
            player.Play();
            Refresh();
            Thread.Sleep(receiptSleep);

            receiptLabel.Text += $"\n  Change          {customerchange.ToString("C")}";
            player.Play();
            Refresh();
            Thread.Sleep(receiptSleep);

            //display a nice message at the end
            receiptLabel.Text += "\n\n  Have a Great Day!";
            playercut.Play();
            Refresh();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            //resets the item inputs
            dashInput.Text = "0";
            poisonInput.Text = "0";
            goldenInput.Text = "0";

            //resets the left calcuations visual
            subtotalStarPicture.Visible = false;
            taxStarPicture.Visible = false;
            totalStarPicture.Visible = false;
            subtotalCoinPicture.Visible = false;
            taxCoinPicture.Visible = false;
            totalCoinPicture.Visible = false;

            starsSubtotalOutput.Text = "";
            coinsSubtotalOutput.Text = "";
            starsTaxOutput.Text = "";
            coinsTaxOutput.Text = "";
            starsTotalOutput.Text = "";
            coinsTotalOutput.Text = "";

            //resets the change user total input
            starPaymentInput.Text = "";
            coinPaymentInput.Text = "";

            //resets the change diplay
            changeStarPicture.Visible = false;
            changeCoinPicture.Visible = false;
            starsChangeOutput.Text = "";
            coinsChangeOutput.Text = "";

            //resets the receipt
            receiptLabel.Text = "";

            //locks any unused buttons and unlocks the calculate total button
            changeButton.Enabled = false;
            printButton.Enabled = false;
            totalButton.Enabled = true;

            //resets variables
            dash = 0;
            poison = 0;
            golden = 0;

            subtotal = 0;
            tax = 0;
            total = 0;

            coins = 0;
            stars = 0;
            customertotal = 0;
            customerchange = 0;

            starsubtotal = 0;
            coinsubtotal = 0;

            startax = 0;
            cointax = 0;

            startotal = 0;
            cointotal = 0;

            customerstars = 0;
            customercoins = 0;
        }
    }
}
