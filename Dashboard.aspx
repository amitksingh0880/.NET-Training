<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="CountryCodeApplication.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <style>
    :root {
        --primary-color: #2C3E50;
        --secondary-color: #18BC9C;
        --danger-color: #E74C3C;
        --warning-color: #F39C12;
        --light-bg: #ECF0F1;
        --white: #fff;
        --gray-light: #BDC3C7;
        --shadow-color: rgba(0, 0, 0, 0.1);
        --border-radius: 12px;
        --font-size-base: 16px;
    }

    /* Main dashboard container */
    .dashboard-container {
        margin: 0 auto;
        padding: 30px;
        background-color: var(--white);
        box-shadow: 0 6px 18px var(--shadow-color);
        border-radius: var(--border-radius);
        max-width: 1100px;
        width: 100%;
        transition: all 0.3s ease;
    }

    .dashboard-container:hover {
        box-shadow: 0 10px 25px var(--shadow-color);
    }

    /* Header section */
    .header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        background-color: var(--primary-color);
        color: var(--white);
        padding: 30px 25px;
        border-radius: var(--border-radius) var(--border-radius) 0 0;
        margin-bottom: 40px;
    }

    .header h2 {
        margin: 0;
        font-size: 1.75rem;
        font-weight: 700;
    }

    /* Buttons */
    .btn {
        padding: 15px 30px;
        font-weight: 600;
        font-size: 1.2rem;
        border: none;
        border-radius: var(--border-radius);
        cursor: pointer;
        transition: background-color 0.3s ease, transform 0.3s ease;
    }

    .btn-logout {
        background-color: var(--danger-color);
        color: var(--white);
    }

    .btn-logout:hover {
        background-color: #C0392B;
        transform: scale(1.05);
    }

    .btn-deactivate {
        background-color: var(--warning-color);
        color: #2C3E50;
        margin-top: 25px;
    }

    .btn-deactivate:hover {
        background-color: #F1C40F;
        transform: scale(1.05);
    }

    /* Error message container */
    .error-container {
        margin: 20px 0;
    }

    .error-container .error {
        color: #E74C3C;
        font-size: 1.2rem;
        font-weight: 600;
    }

    /* Links section */
    .links {
        margin-top: 40px;
    }

    .links h2 {
        font-size: 1.6rem;
        margin-bottom: 20px;
        font-weight: 600;
        color: var(--primary-color);
    }

    .links a {
        display: block;
        color: var(--primary-color);
        text-decoration: none;
        margin-bottom: 18px;
        font-size: 1.2rem;
        font-weight: 600;
        transition: color 0.3s ease;
    }

    .links a:hover {
        color: var(--secondary-color);
        text-decoration: underline;
    }

    /* Repeater Links */
    .repeater-links {
        list-style: none;
        padding-left: 0;
        font-size:  50px;  /* Increased font size */
        font-weight: 800;   /* Bold font weight for better readability */
        margin-top: 15px;
    }

     .repeater-links li {
         margin-bottom: 15px; /* Increased space between list items */
        font-size:  50px; /* Larger font size */
         font-weight: 700; /* Bold font weight */
         line-height: 1.5; /* Increased line height for better readability */
         color: var(--primary-color);
         transition: color 0.3s ease;
     }

    .repeater-links li:hover {
        color: var(--secondary-color); /* On hover, text color changes to secondary */
        text-decoration: underline;    /* Adds underline effect */
    }

    /* Responsive Design */
    @media (max-width: 768px) {
        .dashboard-container {
            padding: 20px;
        }

        .header {
            flex-direction: column;
            align-items: flex-start;
        }

        .header h2 {
            font-size: 1.4rem;
        }

        .btn {
            font-size: 1rem;
            padding: 12px 24px;
        }

        .links h2 {
            font-size: 50px;
        }

        .repeater-links li {
            font-size:  50px;
            margin-bottom: 12px;
        }
    }
</style>


    <div class="dashboard-container">
        <div class="header">
            <h2>Welcome, 
                <asp:Label ID="lblUsername" runat="server" /> 
                (<asp:Label ID="lblRoleName" runat="server" />)
            </h2>
            <asp:Button ID="Button1" runat="server" Text="Logout" CssClass="btn btn-logout" OnClick="btnLogout_Click" />
        </div>

        <asp:Button ID="Button2" runat="server" Text="Deactivate My Account" CssClass="btn btn-deactivate"
            OnClick="btnDeactivate_Click" 
            OnClientClick="return confirm('Are you sure you want to deactivate your account?');" />

        <div class="links">
            <h2>Your Dashboard Links</h2>
            <div class="error-container">
                <asp:Label ID="lblError" runat="server" CssClass="error" />
            </div>
            <asp:Label ID="CurrentUrl" runat="server" />
            <asp:Repeater ID="rptMenu" runat="server">
                <ItemTemplate>
                    <ul class="repeater-links">
                        <li><a href='<%# Eval("RouteURL") %>'><%# Eval("RouteName") %></a></li>
                    </ul>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <script type="text/javascript">
        (function () {
            let timer;
            const logoutAfter = 20 * 60 * 1000;

            function resetTimer() {
                clearTimeout(timer);
                timer = setTimeout(() => {
                    window.location.href = 'Logout.aspx';
                }, logoutAfter);
            }

            ['load', 'mousemove', 'mousedown', 'keypress', 'scroll', 'touchstart'].forEach(evt =>
                window.addEventListener(evt, resetTimer)
            );

            resetTimer();
        })();
    </script>
</asp:Content>
