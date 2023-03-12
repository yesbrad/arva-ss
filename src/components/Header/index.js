import Heading from "../Heading";

const Header = (props) => {
	return (
		<div>
			<p>Sales Shipment</p>
			<h1>{props.children}</h1>
		</div>
	);
}

export default Header;