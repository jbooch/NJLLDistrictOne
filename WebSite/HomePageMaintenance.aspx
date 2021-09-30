<%@ Page Title="New Jersey District One" Language="C#" MasterPageFile="~/masterMain.master" AutoEventWireup="true" CodeFile="HomePageMaintenance.aspx.cs" ValidateRequest="false" Inherits="HomePageMaintenance"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container">
        <div class="row" style="background-color: lavender">
            <div class="col-sm-12 text-sm-center">
                <span style="font-size: 24px; color: #FF0000">New Jersey District 1 Little League
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">District Administrator - Chris Graham
                </span>
                <br />
                <span style="font-size: 18px; color: #000066; font-family: Calibri">NJ District One Home Page Maintenance</span><br />
            </div>
        </div>
    </div>
    <div class="container">
<!--            <ContentTemplate> -->
                <div class="container">
                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <br />
                            <div class="form-group row">
                                <asp:Label ID="Label2" CssClass="col-sm-2 col-form-label" runat="server" Text="Countdown Message:"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtCountdownMessage" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label1" CssClass="col-sm-2 col-form-label" runat="server" Text="Couuntdown Date:"></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtCountdownDate" CssClass="form-control" runat="server"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtCountdownDate" BehaviorID="CustomCalendarExtender" Format="MM/dd/yyyy" PopupPosition="Right"></ajaxToolkit:CalendarExtender>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtCountdownTime" CssClass="form-control" runat="server"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtCountdownTime" Mask="99:99" MessageValidatorTip="true" MaskType="Time" AcceptAMPM="True"></ajaxToolkit:MaskedEditExtender>
                                    <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1" ControlToValidate="txtCountdownTime" Display="Dynamic" IsValidEmpty="False" EmptyValueMessage="Time is required" InvalidValueMessage="Time is invalid" TooltipMessage="Input a time" EmptyValueBlurredText="*" InvalidValueBlurredMessage="*"></ajaxToolkit:MaskedEditValidator>
                                </div>
                            </div>
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="embed-responsive-1by1">
                                <asp:TextBox ID="txtDefaultMessage" runat="server" Rows="5" TextMode="MultiLine" Width="100%"></asp:TextBox>
<!--                                <script type="text/javascript">
                                    make_wyzz("ctl00_ContentPlaceHolder1_txtDefaultMessage", 800, 300); 
                                </script> -->
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <br />
                            <asp:Button CssClass="btn btn-outline-dark" ID="btnDefaultSave" runat="server" Text="Save" OnClick="btnDefaultSave_Click" />
                            <br />
                        </div>
                    </div>
                </div>
<!--            </ContentTemplate> -->
    </div>
</asp:Content>
