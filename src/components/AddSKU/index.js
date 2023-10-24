import { useState } from "react";
import QtySelector from "../QtySelector";
import "./index.css";

const AddSKU = ({ onAddSKU }) => {
	//8127
	const [SKU, setSKU] = useState("8127");
	const [addAmount, setAddAmount] = useState(1);

	const onInput = (e) => {
		e.preventDefault();
		setSKU(e.currentTarget.value)
	}
	
	return (
		<div className="add-container">
			<form onSubmit={(e) => {e.preventDefault(); onAddSKU(SKU, addAmount); setSKU("");}}>
				<input
					onChange={onInput}
					value={SKU}
					></input>
				<QtySelector value={addAmount} onChange={setAddAmount} />
				<button>Add SKU</button>
			</form>
		</div>
	)
}

export default AddSKU;