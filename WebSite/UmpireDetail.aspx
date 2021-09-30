<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UmpireDetail.aspx.cs" Inherits="UmpireDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <meta name="keywords" content="NewJersey Little League District 1 Softball and Baseball" />
        <meta name="description" content="NewJersey Little League District 1 Softball and Baseball" />
        <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
        <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
        <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
        <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous" />
    <title>Umpire Detail</title>
</head>
<body>
    <div class="container">
        <div class="row" style="background-color: lavender">
            <div class="col-sm-12 text-sm-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Umpire Detail</span><br />
            </div>
        </div>
   </div>
    <div class="container">
        <br />
        <div class="jumbotron p-2">
            <div class="container">
                <div class="row">
                    <div class="col-sm-4">
                        <asp:Image ID="imgUmpire" runat="server" ImageUrl="http://www.llnjone.org/umpires/umpirephotos/DefaultUmpire.gif" />
                    </div>
                    <div class="col-sm-8">
                        <h4 class="display-4">
                            <asp:Label ID="lblFirstName" runat="server"></asp:Label>
                            <asp:Label ID="lblLastName" runat="server"></asp:Label>
                        </h4>
                        <p class="lead">
                            Years as Umpire:<asp:Label ID="lblYears" runat="server"></asp:Label>
                            <br />
                            Home League:
                            <asp:Label ID="lblHomeLeague" runat="server"></asp:Label>
                        </p>
                        <p class="my-4">
                            <asp:Label ID="lblHistory" runat="server"></asp:Label>
                        </p>
                    </div>
                </div>
                <div class="offset-4 col-sm-8">
                    <form id="form1" runat="server">
                        <asp:Button CssClass="btn btn-outline-success" ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Close" />
                    </form>
                </div>
            </div>
        </div>
    </div>
 </body>
</html>
