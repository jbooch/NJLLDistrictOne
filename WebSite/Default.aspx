<%@ Page Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Deafult"  Title="New Jersey District One" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="LLNJONECustome.css" rel="stylesheet" />
    <div class="container">
        <div class="row">
            <div class-="col-sm-3">
                <span class="h3">Upcoming District Games</span>
                <div id="myCardCarousel2" class="carousel slide" data-ride="carousel">
                    <div class="carousel-inner">
                        <asp:Label ID="lblUpcomingGames" runat="server" />
                    </div>
                    <a class="carousel-control-prev" href="#myCardCarousel2" role="button" data-slide="prev">
                        <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                        <span class="sr-only">Previous</span>
                    </a>
                    <a class="carousel-control-next" href="#myCardCarousel2" role="button" data-slide="next">
                        <span class="carousel-control-next-icon" aria-hidden="true"></span>
                        <span class="sr-only">Next</span>
                    </a>
                </div>
            </div>
            <div class="col-sm-8 text-center">
                <div class="jumbotron">
                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <span style="font-size: 42px; color: #FF0000">New Jersey District 1 Little League</span>
                            <br />
                            <span style="font-size: 36px; color: #000066; font-family: Calibri">District Administrator - Chris Graham</span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="offset-3 col-sm-6">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="countdownPanel" runat="server">
                                        <table class="table table-bordered">
                                            <thead class="thead-dark">
                                                <tr>
                                                    <th style="font-size: 26px;" colspan="4">
                                                        <asp:Label ID="lblCountdown" runat="server"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th>Days</th>
                                                    <th>Hr</th>
                                                    <th>Min</th>
                                                    <th>Sec</th>
                                                </tr>
                                            </thead>
                                            <tbody class="table-info">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDays" runat="server"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblhours" runat="server"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblMinutes" runat="server"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblSeconds" runat="server"></asp:Label>
                                                        <asp:Timer ID="tmUpdate0" runat="server" Interval="999" OnTick="tmUpdate_Tick">
                                                        </asp:Timer>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12 text-center">
                <asp:Label ID="lblDefaultHeaderMessage" runat="server" CssClass="h3 text-danger"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12 text-center">
                <div class="col-sm-12 text-center">
                    <div id="myCardCarousel" class="carousel slide" data-ride="carousel">
                        <div class="carousel-inner">
                            <asp:Label ID="CardHTML" runat="server" />
                        </div>
                        <a class="carousel-control-prev" href="#myCardCarousel" role="button" data-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span class="sr-only">Previous</span>
                        </a>
                        <a class="carousel-control-next" href="#myCardCarousel" role="button" data-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                            <span class="sr-only">Next</span>
                        </a>
                    </div>
                </div>

            </div>
        </div>
    </div>
</asp:Content>

