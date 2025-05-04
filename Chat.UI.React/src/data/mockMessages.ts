import { Message } from '../types/Message';

// Sample data for testing UI rendering.
export const mockMessages: Message[] = [
  {
    id: '1',
    type: 'text',
    content: `**Welcome!** This is a *Markdown* message.`,
    userId: '2',
    createdAt: new Date('2023-10-01T10:00:00Z')
  },
  {
    id: '20',
    type: 'text',
    content: '![Alt text](https://www.referenseo.com/wp-content/uploads/2019/03/image-attractive.jpg "a title")',
    userId: '1',
    createdAt: new Date('2023-10-01T10:00:00Z')
  },
  {
    id: '2',
    type: 'image',
    content: 'https://www.referenseo.com/wp-content/uploads/2019/03/image-attractive.jpg',
    userId: '1',
    createdAt: new Date('2023-10-01T10:00:00Z')
  },
  {
    id: '3',
    type: 'chart',
    content: JSON.stringify([
      { name: 'Jan', value: 40 },
      { name: 'Feb', value: 80 },
      { name: 'Mar', value: 65 },
      { name: 'Apr', value: 100 }
    ]),
    metadata: {chartType: 'line'},
    userId: '2',
    createdAt: new Date('2023-10-01T10:00:00Z')
  },
  {
    id: '4',
    type: 'chart',
    content: JSON.stringify([
      { name: 'Q1', value: 300 },
      { name: 'Q2', value: 200 },
      { name: 'Q3', value: 400 },
      { name: 'Q4', value: 250 }
    ]),
    metadata: {chartType: 'bar'},
    userId: '1',
    createdAt: new Date('2023-10-01T10:00:00Z')
  },
  {
    id: '5',
    type: 'chart',
    content: JSON.stringify([
      { name: 'Product A', value: 100 },
      { name: 'Product B', value: 150 },
      { name: 'Product C', value: 200 }
    ]),
    metadata: {chartType: 'area'},
    userId: '2',
    createdAt: new Date('2023-10-01T10:00:00Z')
  },
  {
    id: '6',
    type: 'chart',
    content: JSON.stringify([
      { name: 'Group 1', value: 35 },
      { name: 'Group 2', value: 45 },
      { name: 'Group 3', value: 20 }
    ]),
    metadata: {chartType: 'pie'},
    userId: '1',
    createdAt: new Date('2023-10-01T10:00:00Z')
  },
  {
    id: '7',
    type: 'table',
    content: `| Col1  | Col2  | Col3  | Col4  | Col5  | Col6  | Col7  | Col8  | Col9  | Col10 | Col11 | Col12 |\n|-------|-------|-------|-------|-------|-------|-------|-------|-------|-------|-------|-------|\n| R1C1  | R1C2  | R1C3  | R1C4  | R1C5  | R1C6  | R1C7  | R1C8  | R1C9  | R1C10 | R1C11 | R1C12 |\n| R2C1  | R2C2  | R2C3  | R2C4  | R2C5  | R2C6  | R2C7  | R2C8  | R2C9  | R2C10 | R2C11 | R2C12 |\n| R3C1  | R3C2  | R3C3  | R3C4  | R3C5  | R3C6  | R3C7  | R3C8  | R3C9  | R3C10 | R3C11 | R3C12 |\n| R4C1  | R4C2  | R4C3  | R4C4  | R4C5  | R4C6  | R4C7  | R4C8  | R4C9  | R4C10 | R4C11 | R4C12 |\n| R5C1  | R5C2  | R5C3  | R5C4  | R5C5  | R5C6  | R5C7  | R5C8  | R5C9  | R5C10 | R5C11 | R5C12 |\n| R6C1  | R6C2  | R6C3  | R6C4  | R6C5  | R6C6  | R6C7  | R6C8  | R6C9  | R6C10 | R6C11 | R6C12 |\n| R7C1  | R7C2  | R7C3  | R7C4  | R7C5  | R7C6  | R7C7  | R7C8  | R7C9  | R7C10 | R7C11 | R7C12 |\n| R8C1  | R8C2  | R8C3  | R8C4  | R8C5  | R8C6  | R8C7  | R8C8  | R8C9  | R8C10 | R8C11 | R8C12 |\n| R9C1  | R9C2  | R9C3  | R9C4  | R9C5  | R9C6  | R9C7  | R9C8  | R9C9  | R9C10 | R9C11 | R9C12 |\n| R10C1 | R10C2 | R10C3 | R10C4 | R10C5 | R10C6 | R10C7 | R10C8 | R10C9 | R10C10| R10C11| R10C12 |`, 
    userId: '1',
    createdAt: new Date('2023-10-01T10:00:00Z')
  }
];
