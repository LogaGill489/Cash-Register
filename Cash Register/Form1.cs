using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Media;

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

        //integer to calculate total customers
        int customer = 0;
        public Form1()
        {
            InitializeComponent();

            //automatically sets each value to 0
            dashInput.Text = $"0";
            poisonInput.Text = $"0";
            goldenInput.Text = $"0";
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
                tax = subtotal * taxpercent;
                total = subtotal + tax;
                
                //detects if the amount being bought is above the acceptable limit and gives an error message if true
                if (dash > 200 || poison > 200 || golden > 200)
                {
                    receiptLabel.Text = $"Too Many of an Item Has Been Inserted.";
                    receiptLabel.Text += $"\n\nPlease Buy Less Product.";
                }
                else
                {
                    //subtotal display
                    starssubtotalOutput.Text = $"{Convert.ToInt16(subtotal) / 100}";
                    coinssubtotalOutput.Text = $"{subtotal % 100}";

                    //tax display
                    starstaxOutput.Text = $"{Convert.ToInt16(tax) / 100}";
                    coinstaxOutput.Text = $"{Convert.ToInt16(tax) % 100}";

                    //total display
                    starstotalOutput.Text = $"{Convert.ToInt16(total) / 100}";
                    coinstotalOutput.Text = $"{Convert.ToInt16(total) % 100}";

                    //turns the "calculate change" button on
                    changeButton.Enabled = true;
                    printButton.Enabled = false;

                    //shows stars and coins upon button activation
                    subtotalstarPicture.Visible = true;
                    taxstarPicture.Visible = true;
                    totalstarPicture.Visible = true;
                    subtotalcoinPicture.Visible = true;
                    taxcoinPicture.Visible = true;
                    totalcoinPicture.Visible = true;

                    //resets recipt text if all requirements are meet to procede with the purchase
                    receiptLabel.Text = $"";
                }
            }
            catch
            {
                receiptLabel.Text = $"An Error has Occured.";
                receiptLabel.Text += $"\n\nPlease Fill Each Box With a Value of Numbers.";
            }
        }

        private void changeButton_Click(object sender, EventArgs e)
        {
            try
            {
                coins = Convert.ToDouble(coinpaymentInput.Text);
                stars = Convert.ToDouble(starpaymentInput.Text);

                customertotal = coins + (stars * 100);
                customerchange = customertotal - total;

                //detects if the customers total is above the acceptable limit and gives an error message if true
                if (stars > 200 || coins > 9900)
                {
                    receiptLabel.Text = $"Too Many Stars or Coins Have Been Inserted.";
                    receiptLabel.Text += $"\n\nPlease Insert a Smaller Amount.";
                }
                //checks to confirm that the change is above 0 so that the customer can't buy unless they input enough currency
                else if (customerchange >= 0)
                    {
                        starschangeOutput.Text = $"{Convert.ToInt16(customerchange) / 100}";
                        coinschangeOutput.Text = $"{Convert.ToInt16(customerchange) % 100}";

                        //resets recipt text
                        receiptLabel.Text = $"";

                        //shows stars and coins if there is enough payment
                        changestarPicture.Visible = true;
                        changecoinPicture.Visible = true;

                        //print button becomes enable if critera is met
                        printButton.Enabled = true;
                    }
                    else
                    {
                        //displays that there is not enough payment
                        receiptLabel.Text = $"Not Enough Payment Given.";
                        receiptLabel.Text += $"\n\nPlease Provide More Money!";

                        //stops other buttons from working
                        printButton.Enabled = false;

                        //resets the payment
                        changestarPicture.Visible = false;
                        changecoinPicture.Visible = false;
                        starschangeOutput.Text = $"";
                        coinschangeOutput.Text = $"";
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
            SoundPlayer player = new SoundPlayer(Properties.Resources.receipt);
            SoundPlayer playercut = new SoundPlayer(Properties.Resources.receiptcut);

            //stops the reuse of the calculate, change or print recipt button
            totalButton.Enabled = false;
            changeButton.Enabled = false;
            printButton.Enabled = false;

            //temporary increment for customer counting
            customer = customer + 1;

            //from here to the bottom of the private void is the printing animation of the recipt
            //displays company name, order number, current date and creates a gap for if an item needs to be hidden
            receiptLabel.Text = $"\n\n       Mario Party Sales Inc.";
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
            Thread.Sleep(receiptSleep);

            //display a nice message at the end
            receiptLabel.Text += $"\n\n  Have a Great Day!";
            playercut.Play();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            //resets the item inputs
            dashInput.Text = $"0";
            poisonInput.Text = $"0";
            goldenInput.Text = $"0";

            //resets the left calcuations visual
            subtotalstarPicture.Visible = false;
            taxstarPicture.Visible = false;
            totalstarPicture.Visible = false;
            subtotalcoinPicture.Visible = false;
            taxcoinPicture.Visible = false;
            totalcoinPicture.Visible = false;

            starssubtotalOutput.Text = $"";
            coinssubtotalOutput.Text = $"";
            starstaxOutput.Text = $"";
            coinstaxOutput.Text = $"";
            starstotalOutput.Text = $"";
            coinstotalOutput.Text = $"";

            //resets the change user total input
            starpaymentInput.Text = $"";
            coinpaymentInput.Text = $"";

            //resets the change diplay
            changestarPicture.Visible = false;
            changecoinPicture.Visible = false;
            starschangeOutput.Text = $"";
            coinschangeOutput.Text = $"";

            //resets the receipt
            receiptLabel.Text = $"";

            //locks any unused buttons and unlocks the calculate total button
            changeButton.Enabled = false;
            printButton.Enabled = false;
            totalButton.Enabled = true;
        }
    }
}
