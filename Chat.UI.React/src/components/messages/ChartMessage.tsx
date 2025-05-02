import React from 'react';

interface Props {
  data: string; // Will hold JSON or ID for chart data later
}

const ChartMessage: React.FC<Props> = ({ data }) => {
  return (
    <div className="p-3 rounded bg-blue-50 border border-blue-200 text-blue-800 shadow-sm">
      <div className="font-semibold mb-1">📊 Chart Placeholder</div>
      <code className="text-xs break-all block bg-blue-100 p-2 rounded">{data}</code>
    </div>
  );
};

export default ChartMessage;
