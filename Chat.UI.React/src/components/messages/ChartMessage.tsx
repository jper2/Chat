// import React from 'react';

// interface Props {
//   data: string; // Will hold JSON or ID for chart data later
// }

// const ChartMessage: React.FC<Props> = ({ data }) => {
//   return (
//     <div className="p-3 rounded bg-blue-50 border border-blue-200 text-blue-800 shadow-sm">
//       <div className="font-semibold mb-1">📊 Chart Placeholder</div>
//       <code className="text-xs break-all block bg-blue-100 p-2 rounded">{data}</code>
//     </div>
//   );
// };

// export default ChartMessage;


import React from 'react';
import {
  LineChart, BarChart, AreaChart, PieChart,
  Line, Bar, Area, Pie, Cell,
  XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer
} from 'recharts';

interface Props {
  data: string; // JSON string
  metadata?: {
    chartType?: 'line' | 'bar' | 'area' | 'pie';
  };
  onDelete: () => void; // Callback for delete action
}

const ChartMessage: React.FC<Props> = ({ data, metadata }) => {
  const chartType = metadata?.chartType || 'line';
  const chartTypeCapitalized = chartType.charAt(0).toUpperCase() + chartType.slice(1); // Capitalize first letter
  let parsedData;

  try {
    parsedData = JSON.parse(data);
  } catch (error) {
    return (
      <div className="p-3 rounded bg-red-100 border border-red-200 text-red-800 shadow-sm">
        <div className="font-semibold mb-1">⚠️ Invalid Chart Data</div>
        <code className="text-xs break-all block bg-red-50 p-2 rounded">{data}</code>
      </div>
    );
  }

  const renderChart = () => {
    switch (chartType) {
      case 'bar':
        return (
          <BarChart data={parsedData}>
            <CartesianGrid stroke="#ccc" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="value" fill="#3b82f6" />
          </BarChart>
        );
      case 'area':
        return (
          <AreaChart data={parsedData}>
            <CartesianGrid stroke="#ccc" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip />
            <Area type="monotone" dataKey="value" stroke="#3b82f6" fill="#bfdbfe" />
          </AreaChart>
        );
      case 'pie':
        return (
          <PieChart>
            <Tooltip />
            <Pie
              data={parsedData}
              dataKey="value"
              nameKey="name"
              outerRadius={80}
              fill="#3b82f6"
              label
            >
              {parsedData.map((_: any, i: number) => (
                <Cell key={i} fill={['#3b82f6', '#60a5fa', '#93c5fd', '#bfdbfe'][i % 4]} />
              ))}
            </Pie>
          </PieChart>
        );
      case 'line':
      default:
        return (
          <LineChart data={parsedData}>
            <CartesianGrid stroke="#ccc" />
            <XAxis dataKey="name" />
            <YAxis />
            <Tooltip />
            <Line type="monotone" dataKey="value" stroke="#3b82f6" />
          </LineChart>
        );
    }
  };

  return (
    <div className="p-3 rounded shadow-sm h-64">
      <div className="font-semibold mb-1">📊 {chartTypeCapitalized} Chart</div>
      <ResponsiveContainer width="100%" height="85%">
        {renderChart()}
      </ResponsiveContainer>
    </div>
  );
};

export default ChartMessage;

